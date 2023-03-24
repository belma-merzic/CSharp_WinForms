using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cSharpIntroWinForms
{
    public class Validator
    {
        public static bool ValidirajKontrolu(Control kontrola, ErrorProvider err, string poruka)
        {
            bool valid = true;

            if (kontrola is ComboBox && (kontrola as ComboBox).SelectedIndex < 0)
                valid = false;

            if (valid == false)
            {
                err.SetError(kontrola, poruka);
                return false;
            }
            else
            {
                err.Clear();
                return true;
            }
        }
    }
}
