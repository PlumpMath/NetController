using System;
using System.Threading.Tasks;

namespace TPI
{
    internal class TPIObject
    {
        protected TPIReceiver receiver;

        internal void Initialize(TPIReceiver receiver)
        {
            this.receiver = receiver;
        }

        protected virtual async Task<string> ShowAsync()
        {
            if (!(this is ICanShow))
                throw new ArgumentException(Constants.TPI_Msg_Object_Can_Not_Show);
            return await receiver.GetStringAsync(Constants.TPI_Verb_Show, Name);
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().Name.Remove(0, 3);
            }
        }
    }

    internal class TPISerialNumber : TPIObject, ICanShow
    {
        internal TPISerialNumber()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPIUtcTime : TPIObject, ICanShow
    {
        internal TPIUtcTime()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPIGpsTime : TPIObject, ICanShow
    {
        internal TPIGpsTime()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPIPosition : TPIObject, ICanShow
    {
        internal TPIPosition()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPIVoltages : TPIObject, ICanShow
    {
        internal TPIVoltages()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPITemperature : TPIObject, ICanShow
    {
        internal TPITemperature()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPICommands : TPIObject, ICanShow
    {
        internal TPICommands()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPITrackingStatus : TPIObject, ICanShow
    {
        internal TPITrackingStatus()
        {

        }

        #region ICanShow Members

        public async Task<string> Show()
        {
            return await base.ShowAsync();
        }

        #endregion
    }

    internal class TPITracking : TPIObject, ICanShow, ICanSet
    {
        internal TPITracking()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIGpsSatControls : TPIObject, ICanShow, ICanSet
    {
        internal TPIGpsSatControls()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPISbasSatControls : TPIObject, ICanShow, ICanSet
    {
        internal TPISbasSatControls()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIGlonassSatControls : TPIObject, ICanShow, ICanSet
    {
        internal TPIGlonassSatControls()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIEphemeris : TPIObject, ICanShow
    {
        internal TPIEphemeris()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIAlmanac : TPIObject, ICanShow
    {
        internal TPIAlmanac()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIGpsHealth : TPIObject, ICanShow
    {
        internal TPIGpsHealth()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIGpsUtcData : TPIObject, ICanShow
    {
        internal TPIGpsUtcData()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIGpsIonoData : TPIObject, ICanShow
    {
        internal TPIGpsIonoData()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIGnssData : TPIObject, ICanReset
    {
        internal TPIGnssData()
        {

        }
    }

    internal class TPISystem : TPIObject, ICanReset
    {
        internal TPISystem()
        {

        }
    }

    internal class TPIReferenceFrequency : TPIObject, ICanShow, ICanSet
    {
        internal TPIReferenceFrequency()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIElevationMask : TPIObject, ICanShow, ICanSet
    {
        internal TPIElevationMask()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIPdopMask : TPIObject, ICanShow, ICanSet
    {
        internal TPIPdopMask()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIClockSteering : TPIObject, ICanShow, ICanSet
    {
        internal TPIClockSteering()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIMultipathReject : TPIObject, ICanShow, ICanSet
    {
        internal TPIMultipathReject()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIPPS : TPIObject, ICanShow, ICanSet
    {
        internal TPIPPS()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIAntennaTypes : TPIObject, ICanShow
    {
        internal TPIAntennaTypes()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIAntenna : TPIObject, ICanShow, ICanSet
    {
        internal TPIAntenna()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIRtkControls : TPIObject, ICanShow, ICanSet
    {
        internal TPIRtkControls()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIIoPorts : TPIObject, ICanShow
    {
        internal TPIIoPorts()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIIoPort : TPIObject, ICanShow, ICanSet, ICanDelete
    {
        internal TPIIoPort()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIRefStation : TPIObject, ICanShow, ICanSet
    {
        internal TPIRefStation()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIOmniStarSeed : TPIObject, ICanSet
    {
        internal TPIOmniStarSeed()
        {

        }
    }

    internal class TPIFirmwareVersion : TPIObject, ICanShow
    {
        internal TPIFirmwareVersion()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIFirmwareWarranty : TPIObject, ICanShow, ICanSet
    {
        internal TPIFirmwareWarranty()
        {

        }

        #region ICanShow Members

        public Task<string> Show()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class TPIFirmwareFile : TPIObject, ICanUpload
    {
        internal TPIFirmwareFile()
        {

        }
    }
}
