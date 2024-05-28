<Query Kind="Program">
  <Reference Relative="..\IanAutomation\bin\Debug\net7.0\IanAutomation.dll">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.Apps</Namespace>
</Query>

// Do TheAutomationChallenge

void Main()
{
	FlappyBird Page = null;
	try
	{
		// navigate to Flappy Bird
		Page = new FlappyBird();

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

