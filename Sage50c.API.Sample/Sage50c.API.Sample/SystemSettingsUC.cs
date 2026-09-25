using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using S50cSys22;

namespace Sage50c.API.Sample
{
    public partial class SystemSettingsUC : UserControl
    {
        public SystemSettingsUC()
        {
            InitializeComponent();
        }

        private void BGetSystemSettings_Click(object sender, EventArgs e)
        {
            var systemSettings = APIEngine.SystemSettings;

            TbSystemSettingsList.Text = SerializeByProperty(systemSettings, typeof(SystemSettings)).ToString();
        }

        /// <summary>
        /// Serializes each property on its own so that a COM object without type info
        /// (ITypeInfo::GetTypeAttr E_NOTIMPL) doesn't break the whole serialization.
        /// </summary>
        private static JObject SerializeByProperty(object source, Type declaredType)
        {
            var result = new JObject();

            foreach (var property in GetInterfaceProperties(declaredType))
            {
                try
                {
                    var value = property.GetValue(source);
                    result[property.Name] = value == null ? JValue.CreateNull() : JToken.FromObject(value, APIEngine.Serializer);
                }
                catch (Exception ex)
                {
                    var inner = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
                    result[property.Name] = $"<{inner.GetType().Name}: {inner.Message}>";
                }
            }

            return result;
        }

        /// <summary>
        /// COM interop objects are System.__ComObject at runtime, so properties are read from the
        /// declared interop type and its base interfaces (GetProperties doesn't include inherited interface members).
        /// </summary>
        private static IEnumerable<PropertyInfo> GetInterfaceProperties(Type type)
        {
            return new[] { type }
                .Concat(type.GetInterfaces())
                .SelectMany(t => t.GetProperties())
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                .GroupBy(p => p.Name)
                .Select(g => g.First());
        }
    }
}
