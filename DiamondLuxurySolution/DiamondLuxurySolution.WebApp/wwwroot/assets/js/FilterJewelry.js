$(document).ready(function () {
	$('#seeMoreBtn').on('click', function () {
		$('.product-item.hidden').slice(0, 20).removeClass('hidden');
		if ($('.product-item.hidden').length === 0) {
			$('#seeMoreBtn').hide();
		}
	});

	function filterProducts() {
		var gem = $('#gemFilter').val().toLowerCase();
		var price = $('#priceFilter').val();
		var searchQuery = $('#searchInput').val().toLowerCase();
		var material = $('#materialFilter').val().toLowerCase();

		$('.product-item').each(function () {
			var productGem = $(this).data('gem').toLowerCase();
			var productMaterial = $(this).data('material').toLowerCase();
			var productPrice = parseFloat($(this).data('price'));
			var productName = $(this).find('.IdProduct h9 a').text().toLowerCase();

			var showProduct = true;

			if (gem && !productGem.includes(gem)) {
				showProduct = false;
			}

			if (material && productMaterial !== material) {
				showProduct = false;
			}

			if (price) {
				if (price === '10000000') { // Dưới 10,000,000
					if (productPrice >= 10000000) {
						showProduct = false;
					}
				} else if (price === '100000000') { // Trên 100,000,000
					if (productPrice <= 100000000) {
						showProduct = false;
					}
				} else {
					var priceRange = price.split('-');
					if (priceRange.length === 2) {
						var minPrice = parseFloat(priceRange[0]);
						var maxPrice = parseFloat(priceRange[1]);
						if (productPrice < minPrice || productPrice > maxPrice) {
							showProduct = false;
						}
					}
				}
			}

			if (searchQuery && !productName.includes(searchQuery)) {
				showProduct = false;
			}

			if (showProduct) {
				$(this).fadeIn();
			} else {
				$(this).fadeOut();
			}
		});
	}

	$('#materialFilter, #gemFilter, #priceFilter').on('change', function () {
		filterProducts();
	});

	$('#searchInput').on('input', function () {
		filterProducts();
	});

	$('#searchButton').on('click', function () {
		filterProducts();
	});

	filterProducts();

});
