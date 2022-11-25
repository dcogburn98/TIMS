using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel;

namespace TIMS.Forms.ItemImporting
{
    public partial class DefaultFieldValuesEditor : Form
    {
        public Item defaultItem;
        public DefaultFieldValuesEditor(Item currentDefault)
        {
            InitializeComponent();

            defaultItem = currentDefault;

            productLine.Text = defaultItem.productLine;
            itemName.Text = defaultItem.itemName;
            description.Text = defaultItem.longDescription;
            minimum.Text = defaultItem.minimum.ToString();
            maximum.Text = defaultItem.maximum.ToString();
            onHandQty.Text = defaultItem.onHandQty.ToString();
            listPrice.Text = defaultItem.listPrice.ToString();
            redPrice.Text = defaultItem.redPrice.ToString();
            yellowPrice.Text = defaultItem.yellowPrice.ToString();
            greenPrice.Text = defaultItem.greenPrice.ToString();
            pinkPrice.Text = defaultItem.pinkPrice.ToString();
            bluePrice.Text = defaultItem.bluePrice.ToString();
            cost.Text = defaultItem.replacementCost.ToString();
            minimumAage.Text = defaultItem.minimumAge.ToString();
            locationCode.Text = defaultItem.locationCode.ToString();
            category.Text = defaultItem.category;
            upc.Text = defaultItem.UPC;
            mfgPartNo.Text = defaultItem.manufacturerNumber;
            groupCode.Text = defaultItem.groupCode.ToString();
            velocityCode.Text = defaultItem.velocityCode.ToString();
            supplier.Text = defaultItem.supplier;
            containerCt.Text = defaultItem.itemsPerContainer.ToString();
            stdPkg.Text = defaultItem.standardPackage.ToString();
            dateStocked.Text = defaultItem.dateStocked.ToString("MM/dd/yyyy");
            dateLastReceived.Text = defaultItem.dateLastReceipt.ToString("MM/dd/yyyy");
            taxed.Checked = defaultItem.taxed;
            ageRestricted.Checked = defaultItem.ageRestricted;
            serialized.Checked = defaultItem.serialized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal d;
            int i;
            DateTime g;
            defaultItem = new Item()
            {
                itemNumber = "XXX",
                productLine = productLine.Text == "" ? "XXX" : productLine.Text,
                itemName = itemName.Text,
                longDescription = description.Text,
                minimum = decimal.TryParse(minimum.Text, out d)         == false ? 0 : d,
                maximum = decimal.TryParse(maximum.Text, out d)         == false ? 0 : d,
                onHandQty = decimal.TryParse(onHandQty.Text, out d)     == false ? 0 : d,
                listPrice = decimal.TryParse(listPrice.Text, out d)     == false ? 0 : d,
                redPrice = decimal.TryParse(redPrice.Text, out d)       == false ? 0 : d,
                yellowPrice = decimal.TryParse(yellowPrice.Text, out d) == false ? 0 : d,
                greenPrice = decimal.TryParse(greenPrice.Text, out d)   == false ? 0 : d,
                pinkPrice = decimal.TryParse(pinkPrice.Text, out d)     == false ? 0 : d,
                bluePrice = decimal.TryParse(bluePrice.Text, out d)     == false ? 0 : d,
                replacementCost = decimal.TryParse(cost.Text, out d)    == false ? 0 : d,
                minimumAge = int.TryParse(minimumAage.Text, out i)      == false ? 0 : i,
                locationCode = int.TryParse(locationCode.Text, out i)   == false ? 0 : i,
                category = category.Text                                == "" ? "Default" : category.Text,
                UPC = upc.Text,
                manufacturerNumber = mfgPartNo.Text,
                groupCode = int.TryParse(groupCode.Text, out i)         == false ? 0 : i,
                velocityCode = int.TryParse(velocityCode.Text, out i)   == false ? 0 : i,
                supplier = supplier.Text                                == "" ? "Default" : supplier.Text,
                itemsPerContainer = int.TryParse(containerCt.Text, out i)           == false ? 0 : i,
                standardPackage = int.TryParse(stdPkg.Text, out i)                  == false ? 0 : i,
                dateStocked = DateTime.TryParse(dateStocked.Text, out g)            == false ? DateTime.MinValue : g,
                dateLastReceipt = DateTime.TryParse(dateLastReceived.Text, out g)   == false ? DateTime.MinValue : g,
                taxed = taxed.Checked,
                ageRestricted = ageRestricted.Checked,
                serialized = serialized.Checked
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
