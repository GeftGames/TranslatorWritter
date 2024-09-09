using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslatorWritter{
    public class Source{
        public string Shortcut;
        public string ShortcutID;
        public string Data;

        public Source() { }

        public bool Load(string rawData) { 
            if (rawData == "") return false;
            string[] vars= rawData.Split('|');

            string typeSource=vars[0];
            if (typeSource=="kniha") {
                foreach (string rawVar in vars) {
                    string[] parts=rawVar.Split('=');
                    if (parts[0]=="shortcut") {
                        if (parts[1]=="") return false;
                        Shortcut=parts[1];
                                                
                        var list=vars.ToList();
                        list.Remove(rawVar);
                        Data=string.Join("|", list);

                        return true;
                    }  
                }
            }else if (typeSource=="web") {
                foreach (string rawVar in vars) {
                    string[] parts=rawVar.Split('=');
                    if (parts[0]=="shortcut") {
                        if (parts[1]=="") return false;
                        Shortcut=parts[1];

                        var list=vars.ToList();
                        list.Remove(rawVar);
                        Data=string.Join("|", list);
                        
                        //Data=rawData;
                        return true;
                    }  
                }
            } else if (typeSource=="periodikum") {
                foreach (string rawVar in vars) {
                    string[] parts=rawVar.Split('=');
                    if (parts[0]=="shortcut") {
                        if (parts[1]=="") return false;
                        Shortcut=parts[1];

                        var list=vars.ToList();
                        list.Remove(rawVar);
                        Data=string.Join("|", list);
                        
                        //Data=rawData;
                        return true;
                    }  
                }
            } else if (typeSource=="sncj") {
                Shortcut="sncj"; 
                Data=rawData;
                return true;
            }

            return false;
        }

        public static void BuildSourcesIds(List<Source> Sources) { 
            for (int i=0; i<Sources.Count; i++){
                Source Source = Sources[i];
                Source.ShortcutID= (i+1).ToString();
            }
        }

        public static string SavePacker(List<Source> Sources) {
            string cites="";//"b";
            for (int i=0; i<Sources.Count; i++){
                Source Source = Sources[i];
                cites+=Source.Data;
                //string saveP=data.Replace(Source.Shortcut, Source.ShortcutID);
                //cites+=saveP+"\\n";
                cites+="|sid="+Source.ShortcutID+"\\n";
            }
            if (cites.EndsWith("\\n")) cites=cites.Substring(0, cites.Length-2);
            return cites;
        }
    }
}
