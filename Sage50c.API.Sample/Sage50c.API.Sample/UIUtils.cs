﻿using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using S50cSys22;

namespace Sage50c.API.Sample {
    internal static class UIUtils {
        internal static void FillCountryCombo(ComboBox combo) {
            combo.SuspendLayout();

            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DisplayMember = "CountryName";
            combo.ValueMember = "CountryId";

            var rsCountries = APIEngine.DSOCache.CountryProvider.GetCountriesRS();
            while (!rsCountries.EOF) {
                var country = new CountryCode();
                country.CountryID = (string)rsCountries.Fields["CountryId"].Value;
                country.CountryName = (string)rsCountries.Fields["CountryName"].Value;
                combo.Items.Add(country);
                rsCountries.MoveNext();
            }
            rsCountries.Close();
            rsCountries = null;

            combo.ResumeLayout();
        }

        internal static void FillCurrencyCombo(ComboBox combo) {
            combo.SuspendLayout();

            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DisplayMember = "Description";
            combo.ValueMember = "CurrencyId";

            var rs = APIEngine.DSOCache.CurrencyProvider.GetCurrencyActiveRS();
            while (!rs.EOF) {
                CurrencyDefinition currency = new CurrencyDefinition() {
                    CurrencyID = (string)rs.Fields["CurrencyId"].Value,
                    Description = (string)rs.Fields["Description"].Value
                };
                combo.Items.Add(currency);
                rs.MoveNext();
            }
            rs.Close();
            rs = null;

            combo.ResumeLayout();
        }

        internal static void FillEntityFiscalStatusCombo(ComboBox combo) {
            combo.SuspendLayout();

            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DisplayMember = "Description";
            combo.ValueMember = "EntityFSId";

            var rs = APIEngine.DSOCache.TaxesProvider.GetEntityFiscalStatusRS(ZoneTypeEnum.ztNational);
            while (!rs.EOF) {
                var entityfs = new EntityFiscalStatus();
                entityfs.EntityFiscalStatusID = (short)((int)rs.Fields["EntityFiscalStatusId"].Value);
                entityfs.Description = (string)rs.Fields["Description"].Value;

                combo.Items.Add(entityfs);
                rs.MoveNext();
            }
            rs.Close();
            rs = null;

            combo.ResumeLayout();
        }

        internal static void FillMonthCombo(ComboBox combo) {
            combo.SuspendLayout();

            combo.Items.Clear();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            string[] monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames.Where(month => !string.IsNullOrEmpty(month)).ToArray();

            for (int i = 0; i < monthNames.Length; i++) {
                combo.Items.Add(new Month(i + 1, monthNames[i]));
            }

            combo.DisplayMember = "Display";
            combo.ValueMember = "Value";
            combo.ResumeLayout();
        }
    }
}
