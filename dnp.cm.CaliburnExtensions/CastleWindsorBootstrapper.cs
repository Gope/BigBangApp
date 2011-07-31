using System;
using System.Collections.Generic;
using CaliburnMicro.Framework;
using Castle.Windsor;

namespace dnp.cm.CaliburnExtensions
{
    /// <summary>
    /// Startpunkt für die Nutzung eines Castle Windsor basierten Bootstrappers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CastleWindsorBootstrapper<T> : Bootstrapper<T>
    {
        #region Member 

        private IWindsorContainer _Container = new WindsorContainer();
        protected Dictionary<string, Type> _TypeNames = new Dictionary<string, Type>();

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleWindsorBootstrapper&lt;T&gt;"/> class.
        /// </summary>
        public CastleWindsorBootstrapper()
        {
            LogManager.GetLog = type => new DebugLogger(type);
        }

        #endregion

        #region Properties 

        /// <summary>
        /// Der Container
        /// </summary>
        public IWindsorContainer Container
        {
            get { return _Container; }
            set { _Container = value; }
        }

        #endregion

        #region Resolves 

        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>The located service.</returns>
        protected override object GetInstance(Type service, string key)
        {
            if (service == null)
            {
                if(! _TypeNames.TryGetValue(key, out service))
                {
                    throw new ArgumentException("key");
                }
            }

            return string.IsNullOrEmpty(key)
                       ? Container.Resolve(service)
                       : Container.Resolve(key, service);
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>The located services.</returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            // Gibt aktuell nur ein Objekt zurück. Bei Bedarf ausbauen.
            var instances = new List<object>();
            object resolve = Container.Resolve(service);
            instances.Add(resolve);
            return instances;
        }

        #endregion
    }
}