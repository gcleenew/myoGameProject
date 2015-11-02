using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thalmic.Myo
{
    public class MyoEventArgs : EventArgs
    {
        public MyoEventArgs(Myo myo, DateTime timestamp)
        {
            Myo = myo;
            Timestamp = timestamp;
        }

        public Myo Myo { get; private set; }

        public DateTime Timestamp { get; private set; }
    }

    public class ArmSyncedEventArgs : MyoEventArgs
    {
        public ArmSyncedEventArgs(Myo myo, DateTime timestamp, Arm arm, XDirection xDirection)
            : base(myo, timestamp)
        {
            Arm = arm;
            XDirection = xDirection;
        }

        public Arm Arm { get; private set; }
        public XDirection XDirection { get; private set; }
    }

    public class AccelerometerDataEventArgs : MyoEventArgs
    {
        public AccelerometerDataEventArgs(Myo myo, DateTime timestamp, Vector3 accelerometer)
            : base(myo, timestamp)
        {
            Accelerometer = accelerometer;
        }

        public Vector3 Accelerometer { get; private set; }
    }

    public class GyroscopeDataEventArgs : MyoEventArgs
    {
        public GyroscopeDataEventArgs(Myo myo, DateTime timestamp, Vector3 gyroscope)
            : base(myo, timestamp)
        {
            Gyroscope = gyroscope;
        }

        public Vector3 Gyroscope { get; private set; }
    }

    public class OrientationDataEventArgs : MyoEventArgs
    {
        public OrientationDataEventArgs(Myo myo, DateTime timestamp, Quaternion orientation)
            : base(myo, timestamp)
        {
            Orientation = orientation;
        }

        public Quaternion Orientation { get; private set; }
    }

    public class PoseEventArgs : MyoEventArgs
    {
        public PoseEventArgs(Myo myo, DateTime timestamp, Pose pose)
            : base(myo, timestamp)
        {
            Pose = pose;
        }

        public Pose Pose { get; private set; }
    }

    public class RssiEventArgs : MyoEventArgs
    {
        public RssiEventArgs(Myo myo, DateTime timestamp, sbyte rssi)
            : base(myo, timestamp)
        {
            Rssi = rssi;
        }

        public sbyte Rssi { get; private set; }
    }
}
