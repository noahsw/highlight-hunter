<?php /* this figures out what download link to use */

class DownloadLinkGenerator

{
	var $macURL, $latestMacVersion;
	var $clickOnceAutoURL, $clickOnceFallbackURL, $latestPCVersion;

	var $downloadURL, $downloadEvent, $latestVersion, $size;

	var $user_agent;

	var $host;

	public function __construct()
	{
		$this->setUserAgent();
		$this->setHost();
		$this->setDefaults();
		$this->setDownloadLinks();
	}

	public function getDownloadURL() { return $this->downloadURL; }

	public function getDownloadEvent() { return $this->downloadEvent; }

	public function getLatestVersion() { return $this->latestVersion; }

	public function getPCDownloadURL() { return $this->clickOnceFallbackURL; }

	public function getMacDownloadURL() { return $this->macURL; }

	public function getUserAgent() { return $this->user_agent; }

	private function setDownloadLinks()
	{
		$os = $this->getOperatingSystem();

		$browser = $this->getBrowser();


		if ($os == "Mac OS X")
		{
			$supportedOSX = '/(Intel Mac OS X 10_5|Intel Mac OS X 10.5|Intel Mac OS X 10_4|Intel Mac OS X 10.4)/'; // a safer bet is to explicitly look for 10.5 and 10.4
			if (!preg_match($supportedOSX, $this->user_agent))
			{
				$this->downloadURL = $this->macURL;
				$this->downloadEvent = "mac";
				$this->size = "28";
				$this->latestVersion = $this->latestMacVersion;
			}
		}
		else if ($os == "Windows 7" || $os == "Windows Vista" || $os == "Windows XP" || $os == "Windows 8")
		{
			$Net = new NETFrameworkChecker(); //leave first param blank to detect current user agent
			if($Net->Installed() && $Net->getMajor() >= 3 && $Net->getMinor() >= 5)
			{ // We require .NET 3.5
				$this->downloadURL = $this->clickOnceAutoURL;
				$this->downloadEvent = "pc";
				$this->size = "57";
			}
			else if ($browser == "IE" && ($os == "Windows 7" || $os == "Windows 8") )
			{  // newer browsers like IE9 may not report .NET version, but we know Win7 definitely have ClickOnce and .NET 3.5
				$this->downloadURL = $this->clickOnceAutoURL;
				$this->downloadEvent = "pc";
				$this->size = "57";
			}
			else
			{ // not IE but still on Windows, or a lower version of Windows that may not have .NET 3.5
			   $this->downloadURL = $this->clickOnceFallbackURL;
			   $this->downloadEvent = "pc";
			   $this->size = "57";
			}

			$this->latestVersion = $this->latestPCVersion;
		}
		else if ($os == "Search Bot") // so the cached pages in search engines show a download page
		{
			$this->downloadURL = $this->macURL;
			$this->downloadEvent = "mac";
			$this->size = "28";
			$this->latestVersion = $this->latestMacVersion;
		}

		if (isset($_REQUEST["forceplatform"]))
		{
			if ($_REQUEST["forceplatform"] == "mac")
			{
				$this->downloadURL = $this->macURL;
				$this->downloadEvent = "mac";
			}
			else if ($_REQUEST["forceplatform"] == "pc")
			{
				$this->downloadURL = $this->clickOnceFallbackURL;
				$this->downloadEvent = "pc";
			}

		}

	}

	public function getOperatingSystem()
	{
		$OSList = array
		(
		// Match user agent string with operating systems
		'Windows 3.11' => 'Win16',
		'Windows 95' => '(Windows 95)|(Win95)|(Windows_95)',
		'Windows 98' => '(Windows 98)|(Win98)',
		'Windows 2000' => '(Windows NT 5.0)|(Windows 2000)',
		'Windows XP' => '(Windows NT 5.1)|(Windows XP)',
		'Windows Server 2003' => '(Windows NT 5.2)',
		'Windows Vista' => '(Windows NT 6.0)',
		'Windows 7' => '(Windows NT 6.1)',
		'Windows 8' => '(Windows NT 6.2)',
		'Windows NT 4.0' => '(Windows NT 4.0)|(WinNT4.0)|(WinNT)|(Windows NT)',
		'Windows ME' => 'Windows ME',
		'Open BSD' => 'OpenBSD',
		'Sun OS' => 'SunOS',
		'Linux' => '(Linux)|(X11)',
		'Mac OS X' => '(Mac_PowerPC)|(Macintosh)',
		'QNX' => 'QNX',
		'BeOS' => 'BeOS',
		'OS/2' => 'OS\/2',
		'Search Bot'=>'(nuhk)|(Googlebot)|(Yammybot)|(Openbot)|(Slurp)|(MSNBot)|(bingbot)|(Ask Jeeves\/Teoma)|(Yandex)|(Baidu)|(ia_archiver)'
		);
		// Loop through the array of user agents and matching operating systems

		$os = "";
		foreach($OSList as $CurrOS => $Match)
		{
		   // Find a match
		   if (preg_match('/' . $Match . '/', $this->user_agent))
		   {
			  // We found the correct match
			  $os = $CurrOS;
			  break;
		   }
		}


		return $os;
	}

	public function getBrowser()
	{
		$theUA = strtolower($this->user_agent);
		if(strpos($theUA,'msie')) {
		   $browser = "IE";
		} else {
		   $browser = "Other";
		}
		return $browser;
	}

	private function setDefaults()
	{
		if (isset($_REQUEST["force_noappstore"]))
			$this->macURL = $host . "/installers/HighlightHunter.MacSetup.1.3.0.dmg";
		else
			$this->macURL = "http://itunes.apple.com/us/app/highlight-hunter-free/id521035831?mt=12&ls=1"; //uo=4";
		$this->latestMacVersion = "2.0";

		$this->clickOnceAutoURL = $this->host . "/installers/clickonce/HighlightHunter.application"; // "/clickonce-redirect.php"; //
		$this->clickOnceFallbackURL = $this->host . "/installers/clickonce/HighlightHunterSetup.exe";
		$this->latestPCVersion = "2.1";

	}

	private function setHost()
	{
		$isTest = !(strpos($_SERVER["SERVER_NAME"], "test") === false);

		if ($isTest)
			$this->host = "http://test.highlighthunter.com";
		else
			$this->host = "http://static.highlighthunter.com";

	}

	private function setUserAgent()
	{
		if (isset($_REQUEST["force_user_agent"]))
		{
			$this->user_agent = urldecode($_REQUEST["force_user_agent"]);
		}
		else
			$this->user_agent = $_SERVER['HTTP_USER_AGENT'];
	}

}

class NETFrameworkChecker
{
    //General String / Array holders
    var $original_au,$ua_succesParse,$ua_componants,$ua_dotNetString,$CLRTag = "";

    //IsInstalled
    var $installed = false;

    //Version holders
    public $major = 0,$minor = 0,$build = 0;

    public function __construct($ua = false)
    {
        $this->original_au = $ua !== false ? $ua : $_SERVER['HTTP_USER_AGENT'];
        $this->ParserUserAgent();
    }

    public function Installed(){return (bool)$this->installed;}

    public function AUTag(){return $this->CLRTag;}

    //Version Getters
    public function getMajor(){return $this->major;}
    public function getMinor(){return $this->minor;}
    public function getBuild(){return $this->build;}

    private function ParserUserAgent()
    {
        $this->ua_succesParse = (bool) preg_match('/(?<browser>.+?)\s\((?<components>.*?)\)/',$this->original_au,$this->ua_componants);
        if($this->ua_succesParse)
        {
            $this->ua_componants = explode(';',$this->ua_componants['components']);
            foreach($this->ua_componants as $aComponant)
            {
                $aComponant = trim($aComponant);
                if(substr(strtoupper($aComponant),0,4) == ".NET")
                {
                    //We have .Net Installed
                    $this->installed = true;
                    $this->CLRTag = $aComponant;

                    //Lets make sure we can get the versions
                    $gotVersions = (bool)preg_match("/\.NET.CLR.+?(?<major>[0-9]{1})\.(?<minor>[0-9]{1})\.(?<build>[0-9]+)/si",$aComponant,$versions);
                    if($gotVersions)
                    {
                        $this->major = (int)$versions['major'];
                        $this->minor = (int)$versions['minor'];
                        $this->build = (int)$versions['build'];
                    }
                    break;
                }
            }
        }
    }
}







?>
