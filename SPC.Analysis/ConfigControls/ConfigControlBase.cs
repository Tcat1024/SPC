using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SPC.Analysis.ConfigControls
{
    public partial class ConfigControlBase : DevExpress.XtraEditors.XtraUserControl
    {
        public ConfigControlBase()
        {
            InitializeComponent();
        }
        public virtual event EventHandler OKEvent;
        public virtual event EventHandler CancelEvent;
    }
}
