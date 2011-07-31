using System;
using System.Collections.Generic;
using CaliburnMicro.Framework;
using StructureMap;

namespace dnp.cm.CaliburnExtensions
{
    /// <summary>
    /// Ein StructureMap spezifischer Bootstrapper.
    /// </summary>
    /// <typeparam name="T">Typ der</typeparam>
    public class StructureMapBootstrapper<T> : Bootstrapper<T>
    {
        private Container _Container = new Container();

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapBootstrapper&lt;T&gt;"/> class.
        /// </summary>
        public StructureMapBootstrapper()
        {
            LogManager.GetLog = type => new DebugLogger(type);
        }

        #endregion

        #region Container 

        /// <summary>
        /// Der Container.
        /// </summary>
        public Container Container
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
                return GetInstanceByName(key);

            return string.IsNullOrEmpty(key)
                       ? Container.GetInstance(service)
                       : Container.GetInstance(service, key);
        }

        /// <summary>
        /// Findet ein Objekt anhand seines Namens im Container.
        /// </summary>
        /// <param name="key">Der registrierte Name.</param>
        /// <returns></returns>
        private object GetInstanceByName(string key)
        {
            object requestedInstance = null;
            foreach(var instance in Container.Model.AllInstances)
            {
                if(instance.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                {
                    requestedInstance = Container.GetInstance(instance.PluginType, key);
                    break;
                }
            }

            return requestedInstance;
        }

        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>The located services.</returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            var instances = new List<object>();
            foreach(object instance in Container.GetAllInstances(service))
            {
                instances.Add(instance);
            }

            return instances;
        }

        #endregion
    }
}