using System;
using System.Text;

namespace SyslogNet.Client.Serialization
{
    public abstract class SyslogMessageSerializerBase
    {
        private readonly Encoding encoding;

        protected SyslogMessageSerializerBase(Encoding encoding)
        {
            this.encoding = encoding;
        }

        protected static int CalculatePriorityValue(Facility facility, Severity severity)
        {
            return ((int)facility * 8) + (int)severity;
        }

        public Encoding Encoding
        {
            get { return encoding; }
        }
    }

    public abstract class SyslogMessageSerializerBase<T> : SyslogMessageSerializerBase
        where T : SyslogMessageSerializerBase<T>
    {
        private static readonly Lazy<T> _lazy;

        static SyslogMessageSerializerBase()
        {
            _lazy = new Lazy<T>(() => CreateInstance());
        }

        public static T Default
        {
            get { return _lazy.Value; }
        }

        protected SyslogMessageSerializerBase(Encoding encoding)
            : base(encoding)
        {
        }

        private static T CreateInstance()
        {
#if PORTABLE
            var typeInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(T));
            var ctor = System.Linq.Enumerable.Single(typeInfo.DeclaredConstructors,
                c => !c.IsStatic && c.GetParameters().Length == 0);
            return (T)ctor.Invoke(null);
#else
            return (T)Activator.CreateInstance(typeof(T), true);
#endif
        }
    }
}
