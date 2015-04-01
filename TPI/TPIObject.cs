
namespace TPI
{
    public class TPIObject
    {
        protected TPIReceiver receiver;

        internal void Initialize(TPIReceiver receiver)
        {
            this.receiver = receiver;
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().Name.Remove(0, 3);
            }
        }
    }
    
    public class TPISerialNumber : TPIObject, ICanShow
    {
        internal TPISerialNumber()
        {

        }
    }

    public class TPIUtcTime : TPIObject, ICanShow
    {
        internal TPIUtcTime()
        {

        }
    }

    public class TPIGpsTime : TPIObject, ICanShow
    {
        internal TPIGpsTime()
        {

        }
    }

    public class TPIPosition : TPIObject, ICanShow
    {
        internal TPIPosition()
        {

        }
    }

    public class TPIVoltages : TPIObject, ICanShow
    {
        internal TPIVoltages()
        {

        }
    }

    public class TPITemperature : TPIObject, ICanShow
    {
        internal TPITemperature()
        {

        }
    }

    public class TPICommands : TPIObject, ICanShow
    {
        internal TPICommands()
        {

        }
    }

    public class TPITrackingStatus : TPIObject, ICanShow
    {
        internal TPITrackingStatus()
        {

        }
    }

    public class TPITracking : TPIObject, ICanShow, ICanSet
    {
        internal TPITracking()
        {

        }
    }

    public class TPIGpsSatControls : TPIObject, ICanShow, ICanSet
    {
        internal TPIGpsSatControls()
        {

        }
    }

    public class TPISbasSatControls : TPIObject, ICanShow, ICanSet
    {
        internal TPISbasSatControls()
        {

        }
    }

    public class TPIGlonassSatControls : TPIObject, ICanShow, ICanSet
    {
        internal TPIGlonassSatControls()
        {

        }
    }

    public class TPIEphemeris : TPIObject, ICanShow
    {
        internal TPIEphemeris()
        {

        }
    }

    public class TPIAlmanac : TPIObject, ICanShow
    {
        internal TPIAlmanac()
        {

        }
    }

    public class TPIGpsHealth : TPIObject, ICanShow
    {
        internal TPIGpsHealth()
        {

        }
    }

    public class TPIGpsUtcData : TPIObject, ICanShow
    {
        internal TPIGpsUtcData()
        {

        }
    }

    public class TPIGpsIonoData : TPIObject, ICanShow
    {
        internal TPIGpsIonoData()
        {

        }
    }

    public class TPIGnssData : TPIObject, ICanReset
    {
        internal TPIGnssData()
        {

        }
    }

    public class TPISystem : TPIObject, ICanReset
    {
        internal TPISystem()
        {

        }
    }

    public class TPIReferenceFrequency : TPIObject, ICanShow, ICanSet
    {
        internal TPIReferenceFrequency()
        {

        }
    }

    public class TPIElevationMask : TPIObject, ICanShow, ICanSet
    {
        internal TPIElevationMask()
        {

        }
    }

    public class TPIPdopMask : TPIObject, ICanShow, ICanSet
    {
        internal TPIPdopMask()
        {

        }
    }

    public class TPIClockSteering : TPIObject, ICanShow, ICanSet
    {
        internal TPIClockSteering()
        {

        }
    }

    public class TPIMultipathReject : TPIObject, ICanShow, ICanSet
    {
        internal TPIMultipathReject()
        {

        }
    }

    public class TPIPPS : TPIObject, ICanShow, ICanSet
    {
        internal TPIPPS()
        {

        }
    }

    public class TPIAntennaTypes : TPIObject, ICanShow
    {
        internal TPIAntennaTypes()
        {

        }
    }

    public class TPIAntenna : TPIObject, ICanShow, ICanSet
    {
        internal TPIAntenna()
        {

        }
    }

    public class TPIRtkControls : TPIObject, ICanShow, ICanSet
    {
        internal TPIRtkControls()
        {

        }
    }

    public class TPIIoPorts : TPIObject, ICanShow
    {
        internal TPIIoPorts()
        {

        }
    }

    public class TPIIoPort : TPIObject, ICanShow, ICanSet, ICanDelete
    {
        internal TPIIoPort()
        {

        }
    }

    public class TPIRefStation : TPIObject, ICanShow, ICanSet
    {
        internal TPIRefStation()
        {

        }
    }

    public class TPIOmniStarSeed : TPIObject, ICanSet
    {
        internal TPIOmniStarSeed()
        {

        }
    }

    public class TPIFirmwareVersion : TPIObject, ICanShow
    {
        internal TPIFirmwareVersion()
        {

        }
    }

    public class TPIFirmwareWarranty : TPIObject, ICanShow, ICanSet
    {
        internal TPIFirmwareWarranty()
        {

        }
    }

    public class TPIFirmwareFile : TPIObject, ICanUpload
    {
        internal TPIFirmwareFile()
        {

        }
    }
}
