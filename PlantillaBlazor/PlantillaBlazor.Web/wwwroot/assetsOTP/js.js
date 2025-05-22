$('.digit-group').find('input').each(function () {
	$(this).on('keyup', function (e) {
		var parent = $($(this).parent());
		var numeroRegex = /^\d+$/;
		if (e.keyCode === 8 || e.keyCode === 37) {
			var prev = parent.find('input#' + $(this).data('previous'));

			if (prev.length) {
				$(prev).select();
			}
		} else if (numeroRegex.test($(this).val())) {
			var next = parent.find('input#' + $(this).data('next'));

			var lastC = $(this).val().slice(-1);

			$(this).val(lastC);

			if (next.length) {
				$(next).select();
			} else {
				if (parent.data('autosubmit')) {
					parent.submit();
				}
			}
		}
	});
	$(this).on('focus', function () {
		$(this).select();
	});
});
