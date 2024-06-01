<Query Kind="Program">
  <Reference Relative="..\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\bird.png">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\bird.png</Reference>
  <Reference>&lt;NuGet&gt;\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\cvextern.dll</Reference>
  <Reference>&lt;NuGet&gt;\emgu.cv\4.9.0.5494\lib\netstandard2.0\Emgu.CV.dll</Reference>
  <Reference>&lt;NuGet&gt;\emgu.cv\4.9.0.5494\lib\netstandard2.0\Emgu.CV.xml</Reference>
  <Reference Relative="..\IanAutomation\bin\Debug\net7.0\IanAutomation.dll">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Reference>&lt;NuGet&gt;\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\libusb-1.0.dll</Reference>
  <Reference>&lt;NuGet&gt;\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\opencv_videoio_ffmpeg490_64.dll</Reference>
  <Reference Relative="..\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\pipes.png">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\pipes.png</Reference>
  <Reference Relative="..\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\pipes_reverse.png">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\pipes_reverse.png</Reference>
  <Namespace>Emgu.CV</Namespace>
  <Namespace>Emgu.CV.CvEnum</Namespace>
  <Namespace>Emgu.CV.Structure</Namespace>
  <Namespace>Emgu.CV.Util</Namespace>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.Apps.FlappyBird</Namespace>
  <Namespace>IanAutomation.Apps.FlappyBird.Strategies</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

// Do Flappy Bird

void Main()
{
	FlappyBird Page = null;
	IntPtr unmanagedPointer;
	
	string title = "Flappy Bird";
	
	try
	{
		// navigate to Flappy Bird
		Page = new FlappyBird();
		CvInvoke.NamedWindow(title);
		IStrategy Strategy = new AvoidBottomStrategy(Page);
		
		while (CvInvoke.WaitKey(8) != 'q')
		{
			Mat image = new Mat();
			Page.GetScreenshot(image);
			
			Point?  BirdLocation = Page.DetectBird(image);
			
			//var topPoints = Page.DetectTopPipes(image);
				
			var bottomPoints = Page.DetectBottomPipes(image);
			
			Page.AnnotateBird(image, BirdLocation);
			//Page.AnnotateTopPipes(image, topPoints);
			Page.AnnotateBottomPipes(image, bottomPoints);
				
			CvInvoke.Imshow(title, image);
			Strategy.Strategize();
			
			image.Dispose();
		};
		
		
		
		Console.WriteLine("done");
	}
	catch (Exception e)
	{
		Console.WriteLine(e.Message);
	}
	finally
	{
		Thread.Sleep(2000);
		if (Page != null)
			Page.Shutdown();
		CvInvoke.DestroyAllWindows();
	}
}

