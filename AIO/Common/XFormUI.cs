//using SuperSocket.ClientEngine;
using AIO;
using System;
using System.IO;
using System.Windows.Forms;

public class XFormUI //: Form
{

    //StartNewGameEngine

    //protected delegate void dStartNewGameEngine(string game, string apiKey, string mirror, string currency, string stratFile);

    //protected void startNewGameEngine(string game, string apiKey, string mirror, string currency, string stratFile)
    //{
    //    this.Invoke((MethodInvoker)delegate ()
    //    {
    //        var p = this.Parent as Dashboard;

    //        p.GameEngineGateway(game, apiKey, mirror, currency, stratFile);

    //        this.Close();
    //    });
    //}


    //public void OpenFromScript(string game, string apiKey, string mirror, string currency, string stratFile)
    //{

    //}


    public static string GetCrossStrategyLuaFile(string filename)
    {
        string result = "";
        try
        {

            //var p = AppDomain.CurrentDomain.BaseDirectory;
            var p = System.IO.Path.GetFullPath(Application.ExecutablePath);

            var target = Path.Combine(p, "strategies", filename);

            using (StreamReader sr = new StreamReader(target))
            {
                string tmp = sr.ReadToEnd();
                //richTextBox3.Text = tmp;
            }
        }
        catch (Exception ex)
        {
            // DumpLog(ex.Message, 1);
            // DumpLog(ex.StackTrace, 2);
            MessageBox.Show("Invalid file!");
        }

        return result;
    }

    /*
    private void btnOpenCode_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofdtmp = new OpenFileDialog();
        if (ofdtmp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            try
            {
                using (StreamReader sr = new StreamReader(ofdtmp.FileName))
                {
                    string tmp = sr.ReadToEnd();
                    //richTextBox3.Text = tmp;
                }
            }
            catch (Exception ex)
            {
                // DumpLog(ex.Message, 1);
                // DumpLog(ex.StackTrace, 2);
                MessageBox.Show("Invalid file!");
            }
        }
    }

    private void btnCodeSave_Click(object sender, EventArgs e)
    {
        SaveFileDialog svdtmp = new SaveFileDialog();
        if (svdtmp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            try
            {
                // File.WriteAllText(svdtmp.FileName, richTextBox3.Text);
                MessageBox.Show("Saved!");
            }
            catch (Exception ex)
            {
                //DumpLog(ex.Message, 1);
                // DumpLog(ex.StackTrace, 2);
                MessageBox.Show("Could not save code to file.");
            }
        }
    }
    */
}
