namespace FakeItEasy.Configuration
{
    /// <summary>
    /// Provides configuration for method calls that has a return value.
    /// </summary>
    public interface IRepeatConfiguration<out TInterface> : IHideObjectMembers
    {
        /// <summary>
        /// Specifies the number of times for the configured event.
        /// </summary>
        /// <param name="numberOfTimesToRepeat">The number of times to repeat.</param>
        IThenConfiguration<TInterface> NumberOfTimes(int numberOfTimesToRepeat);
    }
}
