using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.DependencyInjectionSample
{
    public interface IOperation
    {
        int PerformOperation(int a, int b);
    }

    public interface IDoubleOperation
    {
        int PerformDoubleOperation(int a, int b);
    }

    public class Addition : IOperation
    {
        private static Lazy<Addition> instance =
            new Lazy<Addition>(() => new Addition());

        public static Addition Instance
        {
            get { return instance.Value; }
        }

        public int PerformOperation(int a, int b)
        {
            return a + b;
        }
    }

    public class DoubleOperation : IDoubleOperation
    {
        private static Lazy<DoubleOperation> instance =
            new Lazy<DoubleOperation>(() => new DoubleOperation(Addition.Instance));

        public static DoubleOperation Instance
        {
            get { return instance.Value; }
        }

        private IOperation operation;

        public DoubleOperation(IOperation operation)
        {
            this.operation = operation;
        }

        public int PerformDoubleOperation(int a, int b)
        {
            return operation.PerformOperation(
                operation.PerformOperation(a, b),
                operation.PerformOperation(a, b));
        }
    }
}
