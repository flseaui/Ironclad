using System;
using DATA;

namespace ATTRIBUTES
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StepOrderAttribute : Attribute
    {
        public StepOrderAttribute(int stepOrder)
        {
            StepOrder = stepOrder;
        }

        public int StepOrder { get; }
    }
}