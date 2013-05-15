using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMReservation
{
    interface IReserveCOM
    {
        void ReserveCOMHandle(COMItem comItem, bool createInTab);
    }
}
