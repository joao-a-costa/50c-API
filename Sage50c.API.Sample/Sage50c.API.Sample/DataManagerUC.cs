using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Sage50c.API.Sample
{
    public partial class DataManagerUC : UserControl
    {
        public DataManagerUC()
        {
            InitializeComponent();
        }

        private void BGetSettings_Click(object sender, EventArgs e)
        {
            var settings = APIEngine.DataManager.MainProvider.Connection;

            TbSettingsList.Text = JToken.FromObject(settings, APIEngine.Serializer).ToString();
        }
    }
}
