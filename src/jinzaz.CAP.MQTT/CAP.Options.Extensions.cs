using DotNetCore.CAP;
using System;

namespace jinzaz.CAP.MQTT
{
    public static class CapOptionsExtensions
    {
        /// <summary>
        /// Extension to use MQTT with specific host name
        /// </summary>
        /// <param name="options"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public static CapOptions UseMQTT(this CapOptions options, string server)
        {
            return options.UseMQTT(option => { option.Server = server; });
        }
        /// <summary>
        /// Extension to use MQTT with configuration action
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static CapOptions UseMQTT(this CapOptions options, Action<CAPMqttOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            options.RegisterExtension(new MqttCAPOptionsExtension(configure));
            return options;
        }
    }
}
