using System;
using System.Text;

namespace SyslogNet.Client.Serialization
{
    /// <summary>
    /// Base message serializer.
    /// </summary>
    public abstract class SyslogMessageSerializerBase
    {
        private readonly Encoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogMessageSerializerBase"/> class.
        /// </summary>
        /// <param name="encoding">Encoding.</param>
        /// <exception cref="ArgumentNullException">Missing encoding value.</exception>
        protected SyslogMessageSerializerBase(Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            this.encoding = encoding;
        }

        /// <summary>
        /// Calculates the message priority value.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Integer priority value.</returns>
        protected static int CalculatePriorityValue(SyslogMessage message)
        {
#pragma warning disable 618
            return CalculatePriorityValue(message.Facility, message.Severity);
#pragma warning restore 618
        }

        /// <summary>
        /// Calculates the message priority value.
        /// </summary>
        /// <param name="facility">Message facility.</param>
        /// <param name="severity">Message severity.</param>
        /// <returns>Integer priority value.</returns>
        [Obsolete("Use CalculatePriorityValue(SyslogMessage) instead.")]
        protected static int CalculatePriorityValue(Facility facility, Severity severity)
        {
            return ((int)facility * 8) + (int)severity;
        }

        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <value>The encoding used for serialization.</value>
        public Encoding Encoding
        {
            get { return encoding; }
        }
    }

    /// <summary>
    /// Base message serializer.
    /// </summary>
    /// <typeparam name="T">Type of serializer.</typeparam>
    public abstract class SyslogMessageSerializerBase<T> : SyslogMessageSerializerBase
        where T : SyslogMessageSerializerBase<T>
    {
        private static readonly Lazy<T> _lazy;

        static SyslogMessageSerializerBase()
        {
            _lazy = new Lazy<T>(() => CreateInstance());
        }

        /// <summary>
        /// Gets the default instance of <typeparamref name="T"/>.
        /// </summary>
        /// <value>Default serializer instance.</value>
        public static T Default
        {
            get { return _lazy.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogMessageSerializerBase"/> class.
        /// </summary>
        /// <param name="encoding">Encoding.</param>
        /// <exception cref="ArgumentNullException">Missing encoding value.</exception>
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
