using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynDumper.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace RoslynDumper
{
    public class RoslynDumper
    {
        private HashSet<int> IsObjectAdded { get; } = new HashSet<int>();

        public string Dump(object element)
        {
            return
                LocalDeclarationStatement(
                        VariableDeclaration(
                                IdentifierName("var"))
                            .WithVariables(
                                SingletonSeparatedList(
                                    VariableDeclarator(
                                            Identifier(
                                                element.GetFormattedVariableName()
                                            ))
                                        .WithTrailingTrivia(SyntaxTrivia(EndOfLineTrivia, ""))
                                        .WithInitializer(
                                            EqualsValueClause(GetObjectCreationExpression(element))))))
                    .NormalizeWhitespace().ToFullString();
        }

        private SyntaxNodeOrToken GetAssignmentExpression(PropertyInfo propertyInfo, object value)
        {
            ExpressionSyntax expressionSyntax = LiteralExpression(NullLiteralExpression);

            if (value != null)
            {
                try
                {
                    expressionSyntax = value.GetExpressionSyntax();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (expressionSyntax == LiteralExpression(NullLiteralExpression))
                {
                    try
                    {
                        expressionSyntax = ParseExpression(value.ToString());
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }

            SyntaxNodeOrToken assignmentExpression = null;

            if (propertyInfo != null)
            {
                assignmentExpression = AssignmentExpression(
                    SimpleAssignmentExpression,
                    IdentifierName(propertyInfo.Name),
                    expressionSyntax
                );
            }
            else
            {
                assignmentExpression = expressionSyntax;
            }

            return assignmentExpression;
        }

        private SyntaxNodeOrToken[] GetSyntaxNodeOrTokens(object element)
        {
            return element is IEnumerable ? GetSyntaxForEnumerables(element) : GetSyntaxForObjects(element);
        }

        private SyntaxNodeOrToken[] GetSyntaxForEnumerables(object element)
        {
            var syntaxNodeOrTokens = new List<SyntaxNodeOrToken>();

            var elementList = ((IEnumerable)element).Cast<object>().ToList();

            for (var elementIndex = 0; elementIndex < elementList.Count(); elementIndex++)
            {
                if (elementList[elementIndex] == null || elementList[elementIndex].GetType().IsSimpleType())
                {
                    syntaxNodeOrTokens.Add(GetAssignmentExpression(null, elementList[elementIndex]));
                }
                else
                {
                    syntaxNodeOrTokens.Add(GetObjectCreationExpression(elementList[elementIndex]));
                }

                if (elementIndex < elementList.Count() - 1)
                {
                    syntaxNodeOrTokens.Add(Token(CommaToken));
                }
            }

            return syntaxNodeOrTokens.ToArray();
        }

        private SyntaxNodeOrToken[] GetSyntaxForObjects(object element)
        {
            var syntaxNodeOrTokens = new List<SyntaxNodeOrToken>();

            var propertyList = element.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList()
                .Where(x => x.GetMethod != null && x.GetMethod.IsPublic && x.GetMethod.IsStatic == false).ToList();

            var lastProperty = propertyList.LastOrDefault();

            foreach (var property in propertyList)
            {
                object value;
                try
                {
                    value = property.GetValue(element);
                }
                catch (Exception ex)
                {
                    value = $"{{{ex.Message}}}";
                }

                if (value == null || value.GetType().IsSimpleType())
                {
                    syntaxNodeOrTokens.Add(GetAssignmentExpression(property, value));
                }
                else
                {
                    var syntax = GetSyntaxForAssignmentExpression(property, value);

                    if (syntax != null)
                    {
                        syntaxNodeOrTokens.Add(syntax.WithLeadingTrivia(SyntaxTrivia(EndOfLineTrivia, string.Empty)));
                    }
                }

                if (!Equals(property, lastProperty))
                {
                    syntaxNodeOrTokens.Add(Token(CommaToken));
                }
            }

            return syntaxNodeOrTokens.ToArray();
        }

        private SyntaxNodeOrToken GetSyntaxForAssignmentExpression(PropertyInfo propertyInfo, object element)
        {
            SyntaxNodeOrToken syntaxNodeOrToken = null;

            if (element == null || element.GetType().IsSimpleType())
            {
                syntaxNodeOrToken = GetAssignmentExpression(null, element);
            }
            else if (element is IEnumerable)
            {
                syntaxNodeOrToken = AssignmentExpression(
                    SimpleAssignmentExpression,
                    IdentifierName(propertyInfo.Name),
                    GetArrayCreationExpression(element));
            }
            else if (element.GetType().IsEnum)
            {
                syntaxNodeOrToken = AssignmentExpression(
                    SimpleAssignmentExpression,
                    IdentifierName(propertyInfo.Name),
                    IdentifierName($"{propertyInfo.PropertyType.FullName}.{element}"));
            }
            else
            {
                if (IsObjectAdded.Add(element.GetHashCode()))
                {
                    syntaxNodeOrToken = GetObjectCreationExpression(element);
                }
            }

            return syntaxNodeOrToken;
        }

        private ArrayCreationExpressionSyntax GetArrayCreationExpression(object property)
        {
            return ArrayCreationExpression(ArrayType(ParseTypeName(property.GetType().GetFormattedName())))
                .WithInitializer(
                    InitializerExpression(ObjectInitializerExpression,
                        SeparatedList<ExpressionSyntax>(GetSyntaxNodeOrTokens(property)
                        )));
        }

        private ObjectCreationExpressionSyntax GetObjectCreationExpression(object element)
        {
            if (element is IEnumerable enumerable)
            {
                return ObjectCreationExpression(GenericName(enumerable.GetType().GetFormattedName()).WithTypeArgumentList(
                        TypeArgumentList(SingletonSeparatedList<TypeSyntax>(IdentifierName(enumerable.Cast<object>().ToList().First().GetType().Name)))))
                    .WithInitializer(
                        InitializerExpression(ObjectInitializerExpression,
                            SeparatedList<ExpressionSyntax>(GetSyntaxNodeOrTokens(enumerable)
                            )));
            }

            return ObjectCreationExpression(ParseTypeName(element.GetType().Name))
                .WithInitializer(
                    InitializerExpression(ObjectInitializerExpression,
                        SeparatedList<ExpressionSyntax>(GetSyntaxNodeOrTokens(element)
                        )));
        }
    }
}