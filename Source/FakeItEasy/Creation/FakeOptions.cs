namespace FakeItEasy.Creation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using FakeItEasy.Core;

    internal class FakeOptions
    {
        private readonly List<Type> additionalInterfacesToImplement;
        private readonly List<Action<object>> fakeConfigurationActions;
        private readonly List<CustomAttributeBuilder> additionalAttributes;
        private FakeWrapperConfigurator wrapper;

        public FakeOptions()
        {
            this.additionalInterfacesToImplement = new List<Type>();
            this.fakeConfigurationActions = new List<Action<object>>();
            this.additionalAttributes = new List<CustomAttributeBuilder>();
        }

        public static FakeOptions Empty
        {
            get { return new FakeOptions(); }
        }

        public FakeWrapperConfigurator Wrapper
        {
            get
            {
                return this.wrapper;
            }

            set
            {
                this.wrapper = value;
                this.AddFakeConfigurationAction(fake => this.wrapper.ConfigureFakeToWrap(fake));
            }
        }

        public IEnumerable<object> ArgumentsForConstructor { get; set; }

        public IEnumerable<Type> AdditionalInterfacesToImplement
        {
            get { return this.additionalInterfacesToImplement; }
        }

        public IEnumerable<Action<object>> FakeConfigurationActions
        {
            get { return this.fakeConfigurationActions; }
        }

        public IEnumerable<CustomAttributeBuilder> AdditionalAttributes
        {
            get { return this.additionalAttributes; }
        }

        public void AddInterfaceToImplement(Type interfaceType)
        {
            this.additionalInterfacesToImplement.Add(interfaceType);
        }

        public void AddFakeConfigurationAction(Action<object> action)
        {
            this.fakeConfigurationActions.Add(action);
        }

        public void AddAttribute(CustomAttributeBuilder attribute)
        {
            this.additionalAttributes.Add(attribute);
        }
    }
}