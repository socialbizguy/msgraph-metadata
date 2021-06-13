
$results = [System.Collections.ArrayList]@()

for ($i = 0; $i -lt 450; $i++) {

$entityCount =  select-Xml -XPath '//edm:EntityType' -Path ..\clean_beta_metadata\cleanMetadataWithDescriptionsbeta.xml `
 -Namespace @{ edm="http://docs.oasis-open.org/odata/ns/edm" } | measure

 $complexCount =  select-Xml -XPath '//edm:ComplexType' -Path ..\clean_beta_metadata\cleanMetadataWithDescriptionsbeta.xml `
 -Namespace @{ edm="http://docs.oasis-open.org/odata/ns/edm" } | measure

 $enumCount =  select-Xml -XPath '//edm:EnumType' -Path ..\clean_beta_metadata\cleanMetadataWithDescriptionsbeta.xml `
 -Namespace @{ edm="http://docs.oasis-open.org/odata/ns/edm" } | measure


 git checkout HEAD~

$CreatedTime = git show -s --format=%ci 


$results.Add( @{ "entities" = $entityCount.Count; 
                 "complex" = $complexCount.Count; 
                 "enum" = $enumCount.Count; 
                 "Created" = $CreatedTime } )
}

$results | select entities, complex, enum, created | ft

git checkout master