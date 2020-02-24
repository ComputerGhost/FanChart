<?php
require_once 'include/header.php';
require_once 'include/functions.php';

// Variables we'll be filling up
$monitored_items = [];
$monitored_types = [];

$pdo = createPDO();

// Get items being monitored
$statement = $pdo->query('SELECT id,site,type,title,url FROM monitored_items ORDER BY site,type,title');
foreach ($statement->fetchAll() as $row) {
	$monitored_items[] = [
		'id' => $row['id'],
		'site' => $row['site'],
		'type' => $row['type'],
		'title' => $row['title'],
		'url' => $row['url'],
		'properties' => []
	];
	$monitored_types[$row['site'] . ' ' . $row['type']] = true;
}

// Get each of the properties of each of the items.
$statement = $pdo->prepare('SELECT property,count,daily FROM monitored_properties WHERE monitored_id=? ORDER BY property DESC');
foreach ($monitored_items as &$item) {
	$statement->execute([$item['id']]);
	foreach ($statement->fetchAll() as $row) {
		$item['properties'][] = [
			'property' => $row['property'],
			'count' => $row['count'],
			'daily' => $row['daily']
		];
	}
}

?>
<script>
//(function() {
	
	function findWord(haystack, word) {
		return haystack.indexOf(word) > -1;
	}
	
	function findAllWords(haystack, words) {
		if (words === null)
			return true;
		for (var i = 0; i != words.length; ++i) {
			var word = words[i];
			if (word in ignoredWords)
				continue;
			if (!findWord(haystack, word))
				return false;
		}
		return true;
	}
	
	var ignoredWords = {};
	["and"].forEach(function(word, i) {
		ignoredWords[word] = i;
	});
	
//})();
</script>

<main class="flex-shrink-0 px-3">
	<div class="container">

		<p>EXIDNumbers is a system to monitor EXID's numbers and update Leggos about milestones.  It started out as a Twitter bot (<a target="_blank" href="https://twitter.com/ExidNumbers">@EXIDNumbers</a>) in November 2019, but now there is this web interface to see all of the numbers instead of just the milestones it tweets about.</p>

		<!--<p>These numbers are updated every eight hours&mdash;8:00, 16:00, and midnight GMT.</p>-->
		<p>Updated daily while in beta testing...</p>

		<div class="mb-1">
			<input id="txtSearch" class="form-control" type="search" placeholder="Search site or title...">
		</div>
		<script>
			$(document).ready(function() {
				$('#txtSearch').on('input', function() {
					var words = $(this).val().toLowerCase().match(/\b(\w+)\b/g);
					$('#tblItems tr').each(function() {
						var haystack = $(this).find('.searchable').text().toLowerCase();
						$(this).toggle(findAllWords(haystack, words));
					});
				});
			});
		</script>

		<div class="table-responsive-md">
			<table class='table table-sm'>
				<caption>Raw numbers such as views and likes for EXID online media</caption>
				<thead class='thead-light'>
					<tr>
						<th style="width:140px">Site &amp; Type</th>
						<th>Title</th>
						<th style="width:95px">Property</th>
						<th class="text-center" style="width:100px">Count</th>
						<th class="text-center" style="width:80px">Daily Average</th>
					</tr>
				</thead>
				<tbody id="tblItems">
					<?php foreach ($monitored_items as $item) {
						$properties = array_column($item['properties'], 'property');
						$counts = array_map('formatNumber', array_column($item['properties'], 'count'));
						$daily = array_map('formatNumber', array_column($item['properties'], 'daily'));
						?>
						<tr>
							<td class="searchable"><?php echo htmlentities($item['site'] . ' ' . $item['type'], ENT_QUOTES); ?></td>
							<td class="searchable"><a target="_blank" href="<?php echo htmlentities($item['url'], ENT_QUOTES); ?>"><?php echo htmlentities($item['title'], ENT_QUOTES); ?></a></td>
							<td><?php echo implode('<br>', array_map('htmlentities', $properties, [ENT_QUOTES])); ?></td>
							<td class="text-right"><?php echo implode('<br>', array_map('htmlentities', $counts, [ENT_QUOTES])); ?></td>
							<td class="text-right"><?php echo implode('<br>', array_map('htmlentities', $daily, [ENT_QUOTES])); ?></td>
						</tr>
					<?php }; ?>
				</tbody>
			</table>
		</div>

	</div>
</main>

<?php require_once 'include/footer.php'; ?>