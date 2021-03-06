﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Kurejito.Validation {
    internal static class ReflectionExtensions {
        public static MemberInfo GetMember(this LambdaExpression expression) {
            var memberExp = RemoveUnary(expression.Body);
            return memberExp == null ? null : memberExp.Member;
        }

        public static MemberInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
            var memberExp = RemoveUnary(expression.Body);

            return memberExp == null ? null : memberExp.Member;
        }

        private static MemberExpression RemoveUnary(Expression toUnwrap) {
            if (toUnwrap is UnaryExpression) {
                return ((UnaryExpression) toUnwrap).Operand as MemberExpression;
            }

            return toUnwrap as MemberExpression;
        }
    }
}