﻿MaximumAttendees=100
HardLimit=120
EntryTemplate=#if($item.Data.Name) $enc.Html($item.Data.Name) #else $enc.Html($item.UserName) #end #if($item.Data.Email), <a href="mailto:$enc.Attr($item.Data.Email)">E-Mail</a> #end #if($item.Data.Blog), <a target="_blank" href="$enc.Attr($item.Data.Blog)">Blog</a> #end #if($item.Data.Twitter), <a target="_blank" href="http://twitter.com/$enc.Attr($item.Data.Twitter)/">Twitter</a> #end #if($item.Data.Xing), <a target="_blank" href="http://xing.com/profile/$enc.Attr($item.Data.Xing)">XING</a> #end #if($item.Data.Picture), <a target="_blank" href="$enc.Attr($item.Data.Picture)">Bild</a> #end #if($item.Data.Sponsoring > 0), $enc.Html($item.Data.FormattedSponsoring) &euro; #end