using System;
using System.Collections.Generic;
using System.Linq;

namespace TranslatorWritter {
    internal static class Methods {
        public static bool ExistsWithName(this List<ItemPatternVerb> list, string name) {
            foreach (ItemPatternVerb v in list){ 
                if (v.Name==name) return true;
            }
            return false;
        }
        
        public static bool ExistsWithName(this List<ItemPatternPronoun> list, string name) {
            foreach (ItemPatternPronoun v in list){ 
                if (v.Name==name) return true;
            }
            return false;
        }
 
        public static ItemPatternNoun GetItemWithName(this List<ItemPatternNoun> list, string name) {
            foreach (ItemPatternNoun item in list) {
                if (item.Name == name) {
                    return item;
                }
            }
            return null;
        } 
        
        public static ItemPatternVerb GetItemWithName(this List<ItemPatternVerb> list, string name) {
            foreach (ItemPatternVerb item in list) {
                if (item.Name == name) {
                    return item;
                }
            }
            return null;
        }

        public static List<TranslatingToData> LoadListTranslatingToData(int start, string[] rawData) {
            List<TranslatingToData> list=new List<TranslatingToData>();
            if (FormMain.LoadedVersionNumber<=2) { 
                for (int i=start; i<rawData.Length; i+=2) {
                    if (i<rawData.Length-1) list.Add(new TranslatingToData{ Text=rawData[i], Comment=rawData[i+1]});
                    else if ((i-start)%2==0 && i==rawData.Length-1) 
                        list.Add(new TranslatingToData{ Text=rawData[i], Comment="", Source=""});
                }
            }else if (FormMain.LoadedVersionNumber==3) {  
                 for (int i=start; i<rawData.Length; i+=3) {
                    if (i<rawData.Length-1) list.Add(new TranslatingToData{ Text=rawData[i], Comment=rawData[i+1], Source=rawData[i+2]});
                    else if ((i-start)%2==0 && i==rawData.Length-1) 
                        list.Add(new TranslatingToData{ Text=rawData[i], Comment="", Source=""});
                }
            }
           
            return list;
        }

        public static List<TranslatingToDataWithPattern> LoadListTranslatingToDataWithPattern(int start, string[] rawData) {
            List<TranslatingToDataWithPattern> list=new List<TranslatingToDataWithPattern>();

            if (FormMain.LoadedVersionNumber<=2){ 
                 for (int i=start; i<rawData.Length-2; i+=3) {
                    list.Add(new TranslatingToDataWithPattern{ Body=rawData[i], Pattern=rawData[i+1], Comment=rawData[i+2], Source=""});
                }
            } else if (FormMain.LoadedVersionNumber==3){ 
                for (int i=start; i<rawData.Length-2; i+=4) {
                    string source="";
                    if (i+3<rawData.Length)source=rawData[i+3];
                    list.Add(new TranslatingToDataWithPattern{ Body=rawData[i], Pattern=rawData[i+1], Comment=rawData[i+2], Source=source});
                }                
            }
            return list;
        }

        public static List<TranslatingToData> LoadListTranslatingToDataWithPattern(string text, char separator) {
            string [] data = text.Split(separator);
            List<TranslatingToData> list=new List<TranslatingToData>();
            for (int i=0; i<data.Length; i++) {
                list.Add(new TranslatingToData{ Text=data[i]});
            }
            return list;
        }

        public static bool Contains(this string str, char ch) {
            foreach (char c in str) {
                if (c == ch) return true;
            }
            return false;
        }

        public static bool Contains(this string str, char[] chs) {
            foreach (char s in str) {
                foreach (char c in chs) {
                    if (s == c) return true;
                }
            }
            return false;
        }

        public static string GetUpperCaseEnding(string body) { 
            int len=0;
            for (int i=body.Length-1; i>=0; i--) {
                if (IsUpperCase(body[i])) len++;
                else break;
            }
            return body.Substring(body.Length-len);    
        }

        static bool IsUpperCase(char ch) { 
            switch (ch) { 
                case 'A': return true; 
                case 'Á': return true; 
                case 'B': return true; 
                case 'C': return true; 
                case 'Č': return true; 
                case 'D': return true; 
                case 'Ď': return true; 
                case 'E': return true; 
                case 'É': return true; 
                case 'Ê': return true; 
                case 'Ě': return true; 
                case 'F': return true; 
                case 'G': return true; 
                case 'H': return true; 
                case 'I': return true; 
                case 'Í': return true; 
                case 'J': return true; 
                case 'K': return true; 
                case 'L': return true; 
                case 'Ł': return true; 
                case 'M': return true; 
                case 'N': return true; 
                case 'Ň': return true; 
                case 'O': return true; 
                case 'Ó': return true; 
                case 'Ô': return true; 
                case 'P': return true; 
                case 'Q': return true; 
                case 'R': return true; 
                case 'Ř': return true; 
                case 'Ŕ': return true; 
                case 'S': return true; 
                case 'Š': return true; 
                case 'T': return true; 
                case 'Ť': return true; 
                case 'U': return true; 
                case 'Ů': return true; 
                case 'Ú': return true; 
                case 'V': return true; 
                case 'W': return true; 
                case 'X': return true; 
                case 'Y': return true; 
                case 'Ý': return true; 
                case 'Z': return true; 
                case 'Ž': return true;
            }
            return false;
        }

        public static string AddQuestionMark(string str) {
            if (str==null) return"";
            if (str=="-") return str;

            if (str.Contains(',')) { 
                string ret="";
                foreach (string s in str.Split(',')) { 
                    ret += s+"?,";
                }
                return ret.Substring(0, ret.Length-1);
            }
            return str+"?";
        }

        public static string ConvertStringToPhonetics(string src) { 
            if (src==null) return "";
            return src
                .Replace("di","ďi")
                .Replace("ni","ňi")
                .Replace("ti","ťi")
                .Replace("dí","ďí")
                .Replace("ní","ňí")
                .Replace("tí","ťí")
                .Replace("ně","ňe")
                .Replace("tě","ťe")
                .Replace("dě","ďe")
                .Replace("bě","bje")
                .Replace("pě","pje")
                .Replace("ů","ú")
                .Replace("x","ks")

                .Replace("DI","ĎI")
                .Replace("NI","ŇI")
                .Replace("TI","ŤI")
                .Replace("DÍ","ĎÍ")
                .Replace("NÍ","ŇÍ")
                .Replace("TÍ","ŤÍ")
                .Replace("NĚ","ŇE")
                .Replace("TĚ","ŤE")
                .Replace("DĚ","ĎE")
                .Replace("BĚ","BJE")
                .Replace("PĚ","PJE")
                .Replace("Ů","Ú")
                .Replace("x","ks")
            ;
        }

        public static bool Contains(List<TranslatingToData> list, char[] chars) { 
            foreach (TranslatingToData data in list){ 
                foreach (char ch in chars) {
                    if (data.Text.Contains(ch)) return true;
                }
            }
            return false;
        }
        public static bool ContainsBody(List<TranslatingToDataWithPattern> list, char[] chars) { 
            foreach (TranslatingToDataWithPattern data in list) { 
                foreach (char ch in chars) {
                    if (data.Body.Contains(ch)) return true;
                }
            }
            return false;
        }

        public static string SavePackerStringMultiple(string src){ 
            if (src.Contains(',')) { 
               // string ret="";
                string[] strings=src.Split(',');
               // bool add=false;
                List<string> add=new List<string>();
                foreach (string str in strings) { 
                    if (!str.Contains(']') && !str.Contains('[') && !str.Contains('?') && !str.Contains(')') && !str.Contains('(') && !str.Contains(' ') && !str.Contains('*')) {// ret+="?,";
                    // else 
                       // ret+=str+","; 
                        add.Add(str);
                    }
                }
                if (add.Count==0) return "?";

                return System.String.Join(",", add);// ret.Substring(0, ret.Length-1);
            } else { 
                if (src.Contains('?')) return "?";
                else return src;                 
            }
        }

        public static string SavePackerStringMultiple(string src, char[] not){ 
            if (src.Contains(',')) { 
               // string ret="";
                string[] strings=src.Split(',');
               // bool add=false;
                List<string> add=new List<string>();
                foreach (string str in strings) {
                    bool con=false;
                    foreach (char ch in not) {
                        if (str.Contains(ch)){ 
                            con=true;
                            break;    
                        }
                    }
                    if (str.Contains('?'))con=true;
                    if (!con/*!str.Contains('?') && !str.Contains(')') && !str.Contains('(') && !str.Contains(' ') && !str.Contains('*')*/){// ret+="?,";
                    // else 
                        add.Add(str);    
                        //add=true;
                    }
                }
                if (add.Count==0) return "?";
                //if (!add) return "?";
                return System.String.Join(",", add);
               // return ret.Substring(0, ret.Length-1);
            } else { 
                if (src.Contains('?')) return "?";
                else return src;                 
            }
        }

        
        // komprese dat "s|f|?|?|?|?|" => "s|f|?×4|"
        public static List<string> CompressPackerData(List<string> shapes){
            for (int i=0; i<shapes.Count; i++) {
                string s = shapes[i];
                if (s=="?") { 
                    int cnt=GetNumberOfUndefined(i);
                    if (cnt>=3) { // 2 se nevyplatí
                        shapes.RemoveRange(i, cnt);
                        // Compressed unknown data
                        shapes.Insert(i, "?×"+cnt);
                        i++;
                     //   continue;
                    }
                }                
                if (s=="-") { 
                    int cnt=GetNumberOfNotDefined(i);
                    if (cnt>=3) { // 2 se nevyplatí
                        shapes.RemoveRange(i, cnt);
                        // Compressed known data
                        shapes.Insert(i, "-×"+cnt);
                        i++;
                      //  continue;
                    }
                }
            }

            return shapes;

            int GetNumberOfUndefined(int start) {
                int i=start;
                for (; i<shapes.Count; i++) {
                    if (shapes[i]!="?") return i-start;
                }
                return i-start;
            }
            int GetNumberOfNotDefined(int start) {
                int i=start;
                for (; i<shapes.Count; i++) {
                    if (shapes[i]!="-") return i-start;
                }
                return i-start;
            }
        } 

        public static List<(string, ItemTranslatingPattern)> OptimizeNamesToPacker(List<ItemTranslatingPattern> list){
            List<(string, ItemTranslatingPattern)> arr=new System.Collections.Generic.List<(string, ItemTranslatingPattern)>();

            for (int i=0; i<list.Count; i++){
                ItemTranslatingPattern p = list[i];
                string origName=p.Name;
                string newOpName=NumbToBytes(i);
                p.Name=newOpName;
                arr.Add((origName, p));
            }
            return arr;

            string NumbToBytes(int number) { 
                string arr_bytes="";
                for (int i=0; i<(4*8-1)/6; i++) {
                    int shifted=number<<i*6; //2^6=64
                    arr_bytes+=GetBase64(shifted);
                    if (shifted<64) break;
                }
                return arr_bytes;
            }            
        }

        public static ItemTranslatingPattern GetOptimizedNameForPacker(List<(string, ItemTranslatingPattern)> list, string searchPatternName){
            foreach ((string, ItemTranslatingPattern) patternPair in list){ 
                if (searchPatternName==patternPair.Item1) { 
                    return patternPair.Item2;
                }
            }
            return null;
        }
        
        static char GetBase64(int num){
                switch (num){ 
                    case 0: return 'A';
                    case 1: return 'B';
                    case 2: return 'C';
                    case 3: return 'D';
                    case 4: return 'E';
                    case 5: return 'F';
                    case 6: return 'G';
                    case 7: return 'H';
                    case 8: return 'I';
                    case 9: return 'J';
                    case 10: return 'K';
                    case 11: return 'L';
                    case 12: return 'M';
                    case 13: return 'N';
                    case 14: return 'O';
                    case 15: return 'P';
                    case 16: return 'Q';
                    case 17: return 'R';
                    case 18: return 'S';
                    case 19: return 'T';
                    case 20: return 'U';
                    case 21: return 'V';
                    case 22: return 'W';
                    case 23: return 'X';
                    case 24: return 'Y';
                    case 25: return 'Z';
                    case 26: return 'a';
                    case 27: return 'b';
                    case 28: return 'c';
                    case 29: return 'd';
                    case 30: return 'e';
                    case 31: return 'f';
                    case 32: return 'g';
                    case 33: return 'h';
                    case 34: return 'i';
                    case 35: return 'j';
                    case 36: return 'k';
                    case 37: return 'l';
                    case 38: return 'm';
                    case 39: return 'n';
                    case 40: return 'o';
                    case 41: return 'p';
                    case 42: return 'q';
                    case 43: return 'r';
                    case 44: return 's';
                    case 45: return 't';
                    case 46: return 'u';
                    case 47: return 'v';
                    case 48: return 'w';
                    case 49: return 'x';
                    case 50: return 'y';
                    case 51: return 'z';
                    case 52: return '0';
                    case 53: return '1';
                    case 54: return '2';
                    case 55: return '3';
                    case 56: return '4';
                    case 57: return '5';
                    case 58: return '6';
                    case 59: return '7';
                    case 60: return '8';
                    case 61: return '9';
                    case 62: return '+';
                    case 63: return '/';
                }

                return '_';
            }
    }
}
