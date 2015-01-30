using Microsoft.Practices.Unity;

namespace Common.Unity
{
    //************************************************************************
    /// <summary>
    /// UnityContainerの単一のインスタンスを管理する。
    /// </summary>
    //************************************************************************
    public class UnityContainerManager
    {
        private static IUnityContainer s_container = new UnityContainer();

        /// <summary>
        /// UnityContainerの単一のインスタンスを返す
        /// </summary>
        public static IUnityContainer Container
        {
            get { return s_container; }
        } 
    }
}
