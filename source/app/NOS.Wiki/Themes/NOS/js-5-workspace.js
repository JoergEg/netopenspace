$(document).ready(function()
{
	$('#workspaces').droppy('> span > a');
		
	if($.browser.msie)
	{
		var width = $($('.pos8')[0]).width();
		$('li a', '#workspaces').each(function()
		{
			$(this).css('width', width + 'px');
		});
	}
});