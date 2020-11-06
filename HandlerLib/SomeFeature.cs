using NServiceBus.Features;
using NServiceBus.Logging;

namespace HandlerLib
{
    public class SomeFeature : Feature
    {
        private ILog logger = LogManager.GetLogger<SomeFeature>();

        public SomeFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            logger.Info($"{nameof(SomeFeature)} setup invoked");
        }
    }
}