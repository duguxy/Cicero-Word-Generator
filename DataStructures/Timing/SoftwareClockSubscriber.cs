﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures.Timing
{
    public interface SoftwareClockSubscriber
    {
        public void reachedTime(uint time_ms);
    }
}
