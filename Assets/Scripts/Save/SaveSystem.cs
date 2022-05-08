using System.Diagnostics;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Debug = UnityEngine.Debug;

public static class SaveSystem
{
   private static string path = $"{Application.persistentDataPath}/Player.json";
   private static string pathActiveSave = $"{Application.persistentDataPath}/ActiveSaveData.json";

   public static void ActiveSaveGame(bool p_bool)
   {
      BinaryFormatter formatter = new BinaryFormatter();
      
      FileStream stream = new FileStream(pathActiveSave, FileMode.Create);

      ActiveSave data = new ActiveSave(p_bool);

      formatter.Serialize(stream, data);
      stream.Close();
   }

   public static bool ReadActiveSave()
   {
      if (File.Exists(pathActiveSave))
      {
         BinaryFormatter formatter = new BinaryFormatter();
         FileStream stream = new FileStream(pathActiveSave, FileMode.Open);

         ActiveSave data = formatter.Deserialize(stream) as ActiveSave;
         stream.Close();
         
         return data.m_active;
      }

      return false;
   }
   
   public static void SavePlayer(PlayerManager player)
   {
      Debug.Log("Je sauve le player");
      BinaryFormatter formatter = new BinaryFormatter();
      
      FileStream stream = new FileStream(path, FileMode.Create);

      PlayerDataSave data = new PlayerDataSave(player);
      
      formatter.Serialize(stream, data);
      
      stream.Close();
   }

   public static PlayerDataSave LoadPlayer()
   {
      if (File.Exists(path))
      {
         
         BinaryFormatter formatter = new BinaryFormatter();
         FileStream stream = new FileStream(path, FileMode.Open);

         PlayerDataSave data = formatter.Deserialize(stream) as PlayerDataSave;
         stream.Close();
         
         return data;
      }
      else
      {
         Debug.LogError("pas de fichier pour load la sauvegarde");
         return null;
      }
   }
}