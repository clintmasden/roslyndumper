using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RoslynDumper.Extensions
{
    internal static class RoslynExtensions
    {
        internal static ExpressionSyntax GetExpressionSyntax(this object @object)
        {
            switch (@object)
            {
                case bool @bool:
                    return LiteralExpression(@bool ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);

                case DateTime dateTime:
                    if (dateTime == DateTime.MinValue)
                    {
                        return IdentifierName("DateTime.MinValue");
                    }
                    else if (dateTime == DateTime.MaxValue)
                    {
                        return IdentifierName("DateTime.MaxValue");
                    }
                    else
                    {
                        return InvocationExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("DateTime"),
                                    IdentifierName("ParseExact")))
                            .WithArgumentList(
                                ArgumentList(
                                    SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            Argument(
                                                LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal(dateTime.ToString()))),
                                            Token(SyntaxKind.CommaToken),
                                            Argument(
                                                LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal("O"))),
                                            Token(SyntaxKind.CommaToken),
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("CultureInfo"),
                                                    IdentifierName("InvariantCulture"))),
                                            Token(SyntaxKind.CommaToken),
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("DateTimeStyles"),
                                                    IdentifierName("RoundtripKind")))
                                        })));
                    }

                default:
                    return LiteralExpression(@object.GetSyntaxKindFromObject(), @object.GetLiteralTokenFromObject());
            }
        }

        private static SyntaxKind GetSyntaxKindFromObject(this object @object)
        {
            switch (@object)
            {
                case string @string:
                    return SyntaxKind.StringLiteralExpression;

                case char @char:
                    return SyntaxKind.CharacterLiteralExpression;

                case int @int:
                case byte @byte:
                case sbyte @sbyte:
                case float @float:
                case uint @uint:
                case long @long:
                case ulong @ulong:
                case short @short:
                case ushort @ushort:
                case decimal @decimal:
                case double @double:
                    return SyntaxKind.NumericLiteralExpression;

                case byte[] byteArray:
                case DateTime dateTime:
                case Enum @enum:
                    return SyntaxKind.DefaultLiteralExpression;

                default:
                    return SyntaxKind.NullLiteralExpression;
            }
        }

        private static SyntaxToken GetLiteralTokenFromObject(this object @object)
        {
            switch (@object)
            {
                case string @string:
                    return Literal(@string);

                case char @char:
                    return Literal(@char);

                case int @int:
                    return Literal(@int);

                case byte @byte:
                    return Literal(@byte);

                case sbyte @sbyte:
                    return Literal(@sbyte);

                case float @float:
                    return Literal(@float);

                case uint @uint:
                    return Literal(@uint);

                case long @long:
                    return Literal(@long);

                case ulong @ulong:
                    return Literal(@ulong);

                case short @short:
                    return Literal(@short);

                case ushort @ushort:
                    return Literal(@ushort);

                case decimal @decimal:
                    return Literal(@decimal);

                case double @double:
                    return Literal(@double);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}