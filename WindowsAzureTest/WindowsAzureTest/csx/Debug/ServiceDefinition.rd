<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WindowsAzureTest" generation="1" functional="0" release="0" Id="0c6ee8ce-8527-4862-b1a6-692ea450d5b4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WindowsAzureTestGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="MvcWebRole1:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/LB:MvcWebRole1:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="MvcWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MapMvcWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="MvcWebRole1:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MapMvcWebRole1:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="MvcWebRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MapMvcWebRole1Instances" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MapWorkerRole1:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MapWorkerRole1Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:MvcWebRole1:Endpoint1">
          <toPorts>
            <inPortMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapMvcWebRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapMvcWebRole1:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapMvcWebRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1Instances" />
          </setting>
        </map>
        <map name="MapWorkerRole1:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/WorkerRole1/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole1:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/WorkerRole1/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/WorkerRole1Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="MvcWebRole1" generation="1" functional="0" release="0" software="E:\Motifworks\VisualstudioPractise\Test45\WindowsAzureTest\WindowsAzureTest\csx\Debug\roles\MvcWebRole1" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;MvcWebRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;MvcWebRole1&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRole1&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1Instances" />
            <sCSPolicyUpdateDomainMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WorkerRole1" generation="1" functional="0" release="0" software="E:\Motifworks\VisualstudioPractise\Test45\WindowsAzureTest\WindowsAzureTest\csx\Debug\roles\WorkerRole1" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;MvcWebRole1&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRole1&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/WorkerRole1Instances" />
            <sCSPolicyUpdateDomainMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/WorkerRole1UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/WorkerRole1FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="MvcWebRole1UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="WorkerRole1UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="MvcWebRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="WorkerRole1FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="MvcWebRole1Instances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WorkerRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="5fbd80bd-b050-41c9-9b3e-7c72c872cb15" ref="Microsoft.RedDog.Contract\ServiceContract\WindowsAzureTestContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="afe60b7c-afee-4204-a165-d6147f3fdb91" ref="Microsoft.RedDog.Contract\Interface\MvcWebRole1:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/WindowsAzureTest/WindowsAzureTestGroup/MvcWebRole1:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>