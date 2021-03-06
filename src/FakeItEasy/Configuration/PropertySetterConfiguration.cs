namespace FakeItEasy.Configuration
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using FakeItEasy.Core;
    using FakeItEasy.Expressions;

    internal class PropertySetterConfiguration<TValue>
        : IPropertySetterAnyValueConfiguration<TValue>
    {
        private readonly ParsedCallExpression parsedSetterExpression;

        private readonly Func<ParsedCallExpression, IVoidArgumentValidationConfiguration> voidArgumentValidationConfigurationFactory;

        public PropertySetterConfiguration(
            ParsedCallExpression parsedCallExpression,
            Func<ParsedCallExpression, IVoidArgumentValidationConfiguration> voidArgumentValidationConfigurationFactory)
        {
            this.parsedSetterExpression = parsedCallExpression;
            this.voidArgumentValidationConfigurationFactory = voidArgumentValidationConfigurationFactory;
        }

        public IPropertySetterConfiguration To(TValue value) =>
            this.To(() => value);

        public IPropertySetterConfiguration To(Expression<Func<TValue>> valueConstraint)
        {
            var newSetterExpression = this.CreateSetterExpressionWithNewValue(valueConstraint);
            var voidArgumentValidationConfiguration = this.CreateArgumentValidationConfiguration(newSetterExpression);
            return AsPropertySetterConfiguration(voidArgumentValidationConfiguration);
        }

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws(Func<IFakeObjectCall, Exception> exceptionFactory) =>
            AsPropertySetterConfiguration(this.CreateArgumentValidationConfiguration(this.parsedSetterExpression))
                .Throws(exceptionFactory);

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1>(Func<T1, Exception> exceptionFactory) =>
            this.Throws<IPropertySetterConfiguration, T1>(exceptionFactory);

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1, T2>(Func<T1, T2, Exception> exceptionFactory) =>
            this.Throws<IPropertySetterConfiguration, T1, T2>(exceptionFactory);

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1, T2, T3>(Func<T1, T2, T3, Exception> exceptionFactory) =>
            this.Throws<IPropertySetterConfiguration, T1, T2, T3>(exceptionFactory);

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Exception> exceptionFactory) =>
            this.Throws<IPropertySetterConfiguration, T1, T2, T3, T4>(exceptionFactory);

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T>() where T : Exception, new() =>
            this.Throws<IPropertySetterConfiguration, T>();

        public IPropertySetterConfiguration Invokes(Action<IFakeObjectCall> action)
        {
            var voidConfiguration = this.CreateArgumentValidationConfiguration(this.parsedSetterExpression)
                .Invokes(action);

            return AsPropertySetterConfiguration(voidConfiguration);
        }

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> CallsBaseMethod() =>
            AsPropertySetterConfiguration(this.CreateArgumentValidationConfiguration(this.parsedSetterExpression))
                .CallsBaseMethod();

        public UnorderedCallAssertion MustHaveHappened(Repeated repeatConstraint) =>
            this.CreateArgumentValidationConfiguration(this.parsedSetterExpression).MustHaveHappened(repeatConstraint);

        public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> DoesNothing() =>
            AsPropertySetterConfiguration(this.CreateArgumentValidationConfiguration(this.parsedSetterExpression))
                .DoesNothing();

        public IPropertySetterConfiguration WhenArgumentsMatch(Func<ArgumentCollection, bool> argumentsPredicate)
        {
            var voidConfiguration = this.CreateArgumentValidationConfiguration(this.parsedSetterExpression)
                .WhenArgumentsMatch(argumentsPredicate);

            return AsPropertySetterConfiguration(voidConfiguration);
        }

        private static IPropertySetterConfiguration AsPropertySetterConfiguration(
                IVoidConfiguration voidArgumentValidationConfiguration) =>
            new ProperySetterAdapter(voidArgumentValidationConfiguration);

        private ParsedCallExpression CreateSetterExpressionWithNewValue(Expression<Func<TValue>> valueExpression)
        {
            var originalParameterInfos = this.parsedSetterExpression.CalledMethod.GetParameters();
            var parsedValueExpression = new ParsedArgumentExpression(
                valueExpression.Body,
                originalParameterInfos.Last());

            var arguments = this.parsedSetterExpression.ArgumentsExpressions
                .Take(originalParameterInfos.Length - 1)
                .Concat(new[] { parsedValueExpression });

            return new ParsedCallExpression(
                this.parsedSetterExpression.CalledMethod,
                this.parsedSetterExpression.CallTarget,
                arguments);
        }

        private IVoidArgumentValidationConfiguration CreateArgumentValidationConfiguration(
                ParsedCallExpression parsedSetter) =>
            this.voidArgumentValidationConfigurationFactory(parsedSetter);

        private class ProperySetterAdapter : IPropertySetterConfiguration
        {
            private IVoidConfiguration voidConfiguration;

            public ProperySetterAdapter(IVoidConfiguration voidArgumentValidationConfiguration)
            {
                this.voidConfiguration = voidArgumentValidationConfiguration;
            }

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws(Func<IFakeObjectCall, Exception> exceptionFactory) =>
                AsPropertySetterConfiguration(this.voidConfiguration).Throws(exceptionFactory);

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1>(Func<T1, Exception> exceptionFactory) =>
                this.Throws<IPropertySetterConfiguration, T1>(exceptionFactory);

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1, T2>(Func<T1, T2, Exception> exceptionFactory) =>
                this.Throws<IPropertySetterConfiguration, T1, T2>(exceptionFactory);

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1, T2, T3>(Func<T1, T2, T3, Exception> exceptionFactory) =>
                this.Throws<IPropertySetterConfiguration, T1, T2, T3>(exceptionFactory);

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Exception> exceptionFactory) =>
                this.Throws<IPropertySetterConfiguration, T1, T2, T3, T4>(exceptionFactory);

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> Throws<T>() where T : Exception, new() =>
                this.Throws<IPropertySetterConfiguration, T>();

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> CallsBaseMethod() =>
                AsPropertySetterConfiguration(this.voidConfiguration).CallsBaseMethod();

            public UnorderedCallAssertion MustHaveHappened(Repeated repeatConstraint) =>
                this.voidConfiguration.MustHaveHappened(repeatConstraint);

            public IAfterCallConfiguredConfiguration<IPropertySetterConfiguration> DoesNothing() =>
                AsPropertySetterConfiguration(this.voidConfiguration).DoesNothing();

            public IPropertySetterConfiguration Invokes(Action<IFakeObjectCall> action)
            {
                this.voidConfiguration = this.voidConfiguration.Invokes(action);
                return this;
            }
        }
    }
}
