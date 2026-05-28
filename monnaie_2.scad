detail_level = 3;     // 1, 2 ou 3 pour acriver chaque niveau de détail

coin_radius = 30;
coin_height = 3;

border_thickness = 1.5;

text_size = 8;
text_height = 0.8;

$fn = 100;


module public_goods_game() {

    union() {

        // niveau 1 de détail: forme de la pièce
        color("gold")
            cylinder(h = coin_height, r = coin_radius);

        // niveau 2 de détail: bordure de la pièce
        if(detail_level >= 2)

            color("black")
            difference() {
                cylinder(h = coin_height + 0.01, r = coin_radius);

                cylinder(
                    h = coin_height + 0.02,
                    r = coin_radius - border_thickness
                );
            }

        // niveau 3 de détail: texte sur relief sur la pièce
        if(detail_level >= 3)

            color("black")
            translate([0,0,coin_height])

            linear_extrude(height = text_height) {

                translate([0,9,0])
                    text(
                        "Public",
                        size = text_size,
                        font = "Broadway",
                        halign = "center"
                    );

                translate([0,0,0])
                    text(
                        "Goods",
                        size = text_size,
                        font = "Broadway",
                        halign = "center"
                    );

                translate([0,-9,0])
                    text(
                        "Game",
                        size = text_size,
                        font = "Broadway",
                        halign = "center"
                    );
            }
    }
}

public_goods_game();