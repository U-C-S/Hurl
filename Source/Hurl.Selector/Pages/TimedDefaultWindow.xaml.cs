using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

namespace Hurl.Selector.Pages;

public sealed partial class TimedDefaultWindow : Window
{
    WindowManager? windowManager;

    public TimedDefaultWindow()
    {
        this.ExtendsContentIntoTitleBar = true;

        windowManager = WindowManager.Get(this);
        windowManager.IsMaximizable = false;
        windowManager.IsMinimizable = false;
#if DEBUG == false
            windowManager.AppWindow.IsShownInSwitchers = false;
#endif
        windowManager.MinWidth = 500;
        windowManager.MinHeight = 250;

        //window.AppWindow.IsShownInSwitchers = false;
        this.AppWindow.ResizeClient(new Windows.Graphics.SizeInt32(600, 320));

        this.InitializeComponent();
    }
}

