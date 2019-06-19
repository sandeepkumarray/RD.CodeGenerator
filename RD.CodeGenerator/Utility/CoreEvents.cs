using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RD.CodeGenerator
{
    public static class CoreEvents
    {
        public static event EventHandler<ClearTextFieldEventArg> ClearTextFieldEvent;
        public static void ResetClearTextFieldEvent() { ClearTextFieldEvent = null; }
        public static void OnClearTextFieldEvent(object Sender, ClearTextFieldEventArg e)
        {
            if (ClearTextFieldEvent != null)
            {
                ClearTextFieldEvent(Sender, e);
            }
        }
    }

    public class ClearTextFieldEventArg : EventArgs
    {

    }
}
