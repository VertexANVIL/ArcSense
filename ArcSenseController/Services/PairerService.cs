using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ArcSenseController.Services
{
    internal class PairerService
    {
        private DeviceWatcher _watcher;

        private TypedEventHandler<DeviceWatcher, DeviceInformation> _handlerAdded = null;
        //private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> _handlerUpdated = null;
        //private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> _handlerRemoved = null;
        //private TypedEventHandler<DeviceWatcher, object> _handlerEnumCompleted = null;
        //private TypedEventHandler<DeviceWatcher, object> _handlerStopped = null;

        internal void StartWatcher()
        {
            _watcher = DeviceInformation.CreateWatcher(
                "System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\"",
                null,
                DeviceInformationKind.AssociationEndpoint);

            _handlerAdded = async (watcher, deviceInfo) =>
            {
                await TryPairDevice(deviceInfo);
            };
        }

        internal void StopWatcher()
        {

        }

        private async Task<bool> TryPairDevice(DeviceInformation deviceInfo)
        {
            deviceInfo.Pairing.Custom.PairingRequested += CustomOnPairingRequested;
            var result = await deviceInfo.Pairing.Custom.PairAsync(DevicePairingKinds.ConfirmOnly, DevicePairingProtectionLevel.EncryptionAndAuthentication);
            var success = result.Status == DevicePairingResultStatus.Paired || result.Status == DevicePairingResultStatus.AlreadyPaired;

            return success;
        }

        private void CustomOnPairingRequested(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            //var deferral = args.GetDeferral();

            // TODO: button-press logic - dummied out
            args.Accept();
            //deferral.Complete();
        }
    }
}
