﻿// Straight forward copy of
//   http://simpleinjector.codeplex.com/wikipage?title=ContextDependentExtensions

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SimpleInjector;

public class DependencyContext
{
    internal static readonly DependencyContext Root =
        new DependencyContext(null, null);

    internal DependencyContext(
        Type serviceType,
        Type implementationType)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
    }

    public Type ServiceType { get; private set; }

    public Type ImplementationType { get; private set; }
}

public static class ContextDependentExtensions
{
    public static void RegisterWithContext<TService>(
        this Container container,
        Func<DependencyContext, TService> contextBasedFactory)
        where TService : class
    {
        Func<TService> rootTypeInstanceCreator =
            () => contextBasedFactory(DependencyContext.Root);

        // Allow TService to be resolved when calling 
        // Container.GetInstance<TService>());
        container.Register<TService>(rootTypeInstanceCreator);

        // Allow the Func<DependencyContext, TService> to be 
        // injected into transient parent types.
        container.ExpressionBuilt += (sender, e) =>
            {
                var rewriter = new DependencyContextRewriter
                    {
                        DelegateToReplace = rootTypeInstanceCreator,
                        ContextBasedFactory = contextBasedFactory,
                        ServiceType = e.RegisteredServiceType
                    };

                e.Expression = rewriter.Visit(e.Expression);
            };
    }

    sealed class DependencyContextRewriter
        : ExpressionVisitor
    {
        readonly List<Expression> parents =
            new List<Expression>();

        public object DelegateToReplace { get; set; }

        public object ContextBasedFactory { get; set; }

        public Type ServiceType { get; set; }

        Expression Parent
        {
            get
            {
                return this.parents.Count < 2
                           ? null
                           : this.parents[this.parents.Count - 2];
            }
        }

        public override Expression Visit(Expression node)
        {
            this.parents.Add(node);

            node = base.Visit(node);

            this.parents.RemoveAt(this.parents.Count - 1);

            return node;
        }

        protected override Expression VisitInvocation(
            InvocationExpression node)
        {
            var parent = this.Parent as NewExpression;

            if (parent != null &&
                this.IsRootTypeContextRegistration(node))
            {
                var context = new DependencyContext(
                    this.ServiceType, parent.Type);

                return Expression.Invoke(
                    Expression.Constant(this.ContextBasedFactory),
                    Expression.Constant(context));
            }

            return base.VisitInvocation(node);
        }

        bool IsRootTypeContextRegistration(
            InvocationExpression node)
        {
            if (node.Expression.NodeType != ExpressionType.Constant)
            {
                return false;
            }

            var constantValue =
                ((ConstantExpression) node.Expression).Value;

            return constantValue == this.DelegateToReplace;
        }
    }
}
