<Query Kind="Program">
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\bird.png</Reference>
  <Reference Relative="..\..\..\..\.nuget\packages\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\cvextern.dll">&lt;NuGet&gt;\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\cvextern.dll</Reference>
  <Reference Relative="..\..\..\..\.nuget\packages\emgu.cv\4.9.0.5494\lib\netstandard2.0\Emgu.CV.dll">&lt;NuGet&gt;\emgu.cv\4.9.0.5494\lib\netstandard2.0\Emgu.CV.dll</Reference>
  <Reference Relative="..\..\..\..\.nuget\packages\emgu.cv\4.9.0.5494\lib\netstandard2.0\Emgu.CV.xml">&lt;NuGet&gt;\emgu.cv\4.9.0.5494\lib\netstandard2.0\Emgu.CV.xml</Reference>
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Reference>F:\projects_csharp\IanNet\bin\Debug\net7.0\IanNet.deps.json</Reference>
  <Reference>F:\projects_csharp\IanNet\bin\Debug\net7.0\IanNet.dll</Reference>
  <Reference>F:\projects_csharp\IanNet\bin\Debug\net7.0\IanNet.pdb</Reference>
  <Reference Relative="..\..\..\..\.nuget\packages\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\libusb-1.0.dll">&lt;NuGet&gt;\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\libusb-1.0.dll</Reference>
  <Reference Relative="..\..\..\..\.nuget\packages\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\opencv_videoio_ffmpeg490_64.dll">&lt;NuGet&gt;\emgu.cv.runtime.windows\4.9.0.5494\runtimes\win-x64\native\opencv_videoio_ffmpeg490_64.dll</Reference>
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\pipes.png</Reference>
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\pipes_reverse.png</Reference>
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\Apps\FlappyBird\Images\ScoreBox.png</Reference>
  <Namespace>Emgu.CV</Namespace>
  <Namespace>Emgu.CV.CvEnum</Namespace>
  <Namespace>Emgu.CV.Structure</Namespace>
  <Namespace>Emgu.CV.Util</Namespace>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.Apps.FlappyBird</Namespace>
  <Namespace>IanAutomation.Apps.FlappyBird.Strategies</Namespace>
  <Namespace>IanNet</Namespace>
  <Namespace>IanNet.Neat</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

// Do Flappy Bird

void Main()
{
	FlappyBird Page = null;
	var neat = new NeatManager();
	
	string title = "Flappy Bird";
	Stopwatch frameStopwatch = new Stopwatch();
	double fps = 60.0;
	int interval = (int)(1000 / fps); // Calculate delay between frames in milliseconds
	
	string bestInGenerationId = "";
	float bestInGenerationScore = 0;
	string filepath = @"F:\projects_csharp\FlappyBirdWithComputerVision\Generations\";
	string lastCompletedGeneration = GetLastCompletedGeneration(filepath);
	var lastCompletedGenerationPath = Path.Join(filepath, lastCompletedGeneration);
	if (lastCompletedGeneration == null)
	{
		Console.WriteLine("No generations were completed. Just start over.");
		return;
	}
	
	SeedPreviousGeneration(lastCompletedGenerationPath, neat);
	
	bool keepGoing = true;
	try
	{
		// navigate to Flappy Bird
		Page = new FlappyBird();
		CvInvoke.NamedWindow(title);
		IStrategy Strategy = new ToyNeuralNetworkStrategy(Page);
		
		Console.WriteLine("here");
		
		// create a new network every time we start over
		while(keepGoing)
		{
			ToyNeuralNetwork brain;
			if (neat.IsGenerationFull())
			{
				neat.NextGeneration();
				bestInGenerationId = "";
				bestInGenerationScore = 0;
			}
			if (neat.IsFirstGeneration())
			{
				brain = new ToyNeuralNetwork(5, 8, 1);
				neat.Add(brain);
			}
			else
			{
				brain = (ToyNeuralNetwork)neat.SpawnChild();
			}
			
			((ToyNeuralNetworkStrategy)Strategy).SetToyNeuralNetwork(brain);
			Page.Restart();
			Page.SetPipeMinGap(200);
			
			Stopwatch ScoreStopwatch = new Stopwatch();
			ScoreStopwatch.Start();
		
			// draw the game as it runs
			while (keepGoing)
			{
				frameStopwatch.Restart();
				
				Mat image = new Mat();
				Page.GetScreenshot(image);
				
				Point?  BirdLocation = Page.DetectBird(image);
				var topHalfPoints = Page.DetectTopHalfPipes(image);
				var bottomHalfPoints = Page.DetectBottomHalfPipes(image);
				Point? ScoreBoxLocation = Page.DetectScoreBox(image);
				
				Page.AnnotateBird(image, BirdLocation);
				Page.AnnotateTopHalfPipes(image, topHalfPoints);
				Page.AnnotateBottomHalfPipes(image, bottomHalfPoints);
				Page.AnnotateScoreBox(image, ScoreBoxLocation);
				
				AnnotateNeatId(image, brain.NeatId);
					
				CvInvoke.Imshow(title, image);
				Strategy.Strategize();
				
				if (Page.IsGameOver(image))
					break;
				
				image.Dispose();
				
				frameStopwatch.Stop();
				var elapsed = frameStopwatch.Elapsed.TotalMilliseconds;
				var waitTime = interval - elapsed;
				var waitTimeInt = waitTime > 0 ? (int)waitTime : 1;
				
				if (CvInvoke.WaitKey(waitTimeInt) == 'q') // Exit if 'q' key is pressed
	                keepGoing = false;
	        }
			
			ScoreStopwatch.Stop();
			neat.SetScore(brain, ScoreStopwatch.ElapsedMilliseconds / 1000f);
			string[] info = brain.NeatId.Split('-');
			string filepath2 = Path.Join(filepath, info[0], info[1]) + ".json";
			brain.Serialize(filepath2);
			brain.Dispose();
			
			if (brain.Score > bestInGenerationScore)
			{
				Console.WriteLine("Best in Generation so far:");
				Console.WriteLine($"{brain.NeatId}: {brain.Score}");
				Console.WriteLine();
				
				bestInGenerationScore = brain.Score;
				bestInGenerationId = brain.NeatId;
			}
		}
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
		foreach(var brain in neat.Neatables)
		{
			((ToyNeuralNetwork)brain).Dispose();
		}
	}
}

public void SeedPreviousGeneration(string generationPath, NeatManager neat)
{
	// for each file...
	foreach(string file in Directory.GetFiles(generationPath))
	{
		neat.Add(new ToyNeuralNetwork(file));
	}
	
	// and set the current generation correctly
	string folderName = Path.GetFileName(generationPath.TrimEnd(Path.DirectorySeparatorChar));
	neat.generation = int.Parse(folderName);
}

public string GetLastCompletedGeneration(string folderpath)
{
	var directories = Directory.GetDirectories(folderpath);
	var folderNames = directories.Select(Path.GetFileName);
	var sortedFolderNames = folderNames.OrderBy(name => 
    {
        int.TryParse(name, out int result);
        return result;
    });
	var lastFolder = sortedFolderNames.Last();
	var files = Directory.GetFiles(Path.Join(folderpath, lastFolder));
	if (files.Count() == 100)
		return lastFolder;
	else if (sortedFolderNames.Count() <= 1)
		return null;
	else
		return sortedFolderNames.ToList()[sortedFolderNames.Count() - 2];
}

public void AnnotateNeatId(Mat image, string text)
{
	CvInvoke.PutText(
		image,
		text,
		new Point(10, image.Height - 20),
		FontFace.HersheyPlain,
		fontScale: 2.3,
		Convert(new Rgb(0, 0, 0)),
		thickness: 2,
		lineType: LineType.AntiAlias
	);
}

public MCvScalar Convert(Rgb Color)
{
	return new MCvScalar(Color.Blue, Color.Green, Color.Red);
}

public void GetNextBrain(IStrategy Strategy, NeatManager neat)
{
	try
	{
		((ToyNeuralNetworkStrategy)Strategy).SetToyNeuralNetwork((ToyNeuralNetwork)neat.Next());
	}
	catch(Exception e)
	{
		if (e.Message == "Out of neatables!")
		{
			neat.NextGeneration();
			((ToyNeuralNetworkStrategy)Strategy).SetToyNeuralNetwork((ToyNeuralNetwork)neat.Next());
		}
		else
		{
			throw;
		}
	}
}