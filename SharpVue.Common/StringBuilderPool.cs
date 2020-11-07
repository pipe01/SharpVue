using System;
using System.Collections.Generic;
using System.Text;

namespace SharpVue.Common
{
    public static class StringBuilderPool
    {
        private static readonly Queue<StringBuilder> Pool = new Queue<StringBuilder>();

        public static IDisposable Rent(out StringBuilder builder)
        {
            lock (Pool)
            {
                if (Pool.Count == 0)
                {
                    builder = new StringBuilder();
                }
                else
                {
                    builder = Pool.Dequeue();
                    builder.Clear();
                }
            }

            return new Returner(builder);
        }

        private class Returner : IDisposable
        {
            private readonly StringBuilder Builder;

            public Returner(StringBuilder builder)
            {
                this.Builder = builder;
            }

            public void Dispose()
            {
                lock (Pool)
                {
                    Pool.Enqueue(Builder);
                }
            }
        }
    }
}
