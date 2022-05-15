using System.Runtime.InteropServices;

namespace 八价兲谛全丰.Pages
{
    class WebView2BridgeClass
    {
    }

    [ComVisible(true)]
    public class Bridge1
    {
        public string Func123(string param) => "Example: " + param;
    }

}
