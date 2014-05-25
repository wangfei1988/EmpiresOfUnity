using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class ControllSettings : ScriptableObject 
{
	public static class Seralizer<Tcolection,T> where Tcolection : ICollection<T> where T : IDictionary
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<System.Enum,System.Enum>[]));

        public static bool Serialize(Tcolection dataObject,string fileName)
        { 
            FileStream fStream;
        if (File.Exists(fileName))
            fStream = File.OpenWrite(fileName);
        else
            fStream = File.Create(fileName);

            serializer.Serialize(fStream,dataObject);
            fStream.Close();
            return true;
        }

        public static object Deseialize(string fileName)
        {
            FileStream fStream=File.OpenRead(fileName);
            return serializer.Deserialize(fStream);
        }

    }

	public Dictionary<System.Enum,System.Enum> PrimaryControlSettings=new Dictionary<System.Enum,System.Enum>();	
    public Dictionary<System.Enum,System.Enum> SecondaryControlSettings=new Dictionary<System.Enum,System.Enum>();	

    public enum EoU_KeySettings : int
    {
        Scrolling_UP=0,
        Scrolling_LEFT,
        Scrolling_DOWN,
        Scrolling_RIGHT,
        fast_Scrolling ,
	    Scrolling_Disabled,
        Camera_Rotate_LEFT,	
	    Camera_Rotate_RIGHT,
        Camera_Zoom_IN,	
		Camera_Zoom_OUT,
        Camera_Switch,		
        Move_Units,			
        Release_Focus,	
        ESC,			
    }

    public void copyLists(List<KeyCode> pri, List<KeyCode> sec)
    {
        PrimaryControlSettings.Clear();
        SecondaryControlSettings.Clear();
        for (int i = 0; i < pri.Count; i++)
			{
			    PrimaryControlSettings.Add((EoU_KeySettings)i,pri[i]);
			}
        for (int i = 0; i < sec.Count; i++)
        {
            SecondaryControlSettings.Add((EoU_KeySettings)i, sec[i]);
        }
    }

    public void SaveSettings()
    {

        Dictionary<System.Enum,System.Enum>[] buffer = new Dictionary<System.Enum,System.Enum>[2];
        buffer[0]=PrimaryControlSettings;
        buffer[1]=SecondaryControlSettings;
        Seralizer<ICollection<Dictionary<System.Enum,System.Enum>>,Dictionary<System.Enum,System.Enum>>.Serialize(buffer, "ControllSettings.dat");
    }

    public void LoadSettings()
    {
        Dictionary<System.Enum, System.Enum>[] buffer = new Dictionary<System.Enum, System.Enum>[2];
        buffer = (Dictionary<System.Enum, System.Enum>[])Seralizer<ICollection<Dictionary<System.Enum, System.Enum>>, Dictionary<System.Enum, System.Enum>>.Deseialize("ControllSettings.dat");
        PrimaryControlSettings = buffer[0];
        SecondaryControlSettings = buffer[1];
    }
}
