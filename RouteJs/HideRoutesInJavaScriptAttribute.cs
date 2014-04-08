namespace RouteJs
{
    using System;

    /// <summary>
    /// Specifies that this controller should not be exposed in the Route JS routes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class HideRoutesInJavaScriptAttribute : Attribute
    {
    }
}
