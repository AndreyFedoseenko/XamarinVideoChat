using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DeviceSpecificApp.Controls
{
    public class SenderEntry : Entry
    {
        public event EventHandler<EventArgs> TextSended;

        public virtual void OnTextSended()
        {
            this.TextSended?.Invoke(this, new EventArgs());
        }
    }
}
