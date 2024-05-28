<Query Kind="Program">
  <Reference Relative="..\IanAutomation\bin\Debug\net7.0\IanAutomation.dll">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.Apps</Namespace>
</Query>

// Do Flappy Bird

void Main()
{
	FlappyBird Page = null;
	try
	{
		// navigate to Flappy Bird
		Page = new FlappyBird();
		
		for (int i=0; i<20; i++)
		{
			Page.Flap();
			Thread.Sleep(200);
		}

		Console.WriteLine("done");
	}
	catch (Exception e)
	{
		Console.WriteLine(e.Message);
	}
	finally
	{
		Thread.Sleep(10000);
		if (Page != null)
			Page.Shutdown();
	}
}

