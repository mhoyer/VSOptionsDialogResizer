using System.Collections.Generic;
using System.Linq;
using SimpleInjector;
using SimpleInjector.Extensions;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer.Addin
{
    public partial class Connect
    {
        internal Container _container;

        internal void InitContainer()
        {
            var assembly = typeof(IOptionsDialogWatcher).Assembly;
            var exportedTypes = assembly.GetExportedTypes();
            var classes = exportedTypes.Where(t => t.IsClass);
            var interfaces = exportedTypes.Where(t => t.IsInterface);

            var registrations = from @interface in interfaces
                                let types = classes.Where(@interface.IsAssignableFrom)
                                where types.Count() == 1
                                select new
                                {
                                    Interface = @interface,
                                    Implementation = types.First()
                                };

            _container = new Container();
            _container.Register<IList<IWindowModifier>>(() => _container.GetAllInstances<IWindowModifier>().ToList());
            _container.RegisterWithContext(CyclicWorkerContexts);
            registrations.ToList().ForEach(r => _container.Register(r.Interface, r.Implementation));
        }

        ICyclicWorker CyclicWorkerContexts(DependencyContext context)
        {
            if (typeof(IOptionsDialogWatcher).IsAssignableFrom(context.ImplementationType))
                return _container.GetInstance<CyclicBackgroundWorker>();

            return _container.GetInstance<CyclicWorker>();
        }
    }
}