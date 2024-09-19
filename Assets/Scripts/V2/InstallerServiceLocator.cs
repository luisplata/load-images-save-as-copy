using Plugins.ServiceLocator;
using UnityEngine;

public class InstallerServiceLocator : MonoBehaviour
{
    private void Awake()
    {
        //search all instances and if there is more than one, destroy itself
        if(FindObjectsOfType<InstallerServiceLocator>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        var _data = new SaveAndLoadData();
        ServiceLocator.Instance.RegisterService<ILoadData>(_data);
        ServiceLocator.Instance.RegisterService<ISaveData>(_data);
        DontDestroyOnLoad(gameObject);
    }
}